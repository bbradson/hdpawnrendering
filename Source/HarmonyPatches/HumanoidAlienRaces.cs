// Copyright (c) 2022 bradson
// This Source Code Form is subject to the terms of the MIT license.
// If a copy of the license was not distributed with this file,
// You can obtain one at https://opensource.org/licenses/MIT/.

using System.Linq;
using System.Reflection;

namespace Fish.HarmonyPatches;

// Manually patching from StaticConstructor. Doing it earlier seems to blow up HAR.
public static class HumanoidAlienRaces
{
	/// <summary>
	/// HAR allows defining varying resolutions for races and stores each of them in their own frameset.
	/// This method here checks the resolution and mesh size to decide whether a new set is necessary or not.
	/// With HD Pawn Rendering overwriting the default resolution and changing it to a higher value
	/// this results in that check always returning false and new sets getting generated every time,
	/// leading to a memory leak. The patch here makes the resolution check return true at all times instead,
	/// which is fine when overriding all the resolutions to the same value like HD Pawn Rendering does.
	/// The mesh size check remains the same. That's necessary for drawSizes, which are independent of
	/// texture dimensions.
	/// </summary>
	[HarmonyPatch]
	public static class HarmonyPatches_TextureAtlasSameRace
	{
		public static MethodInfo TargetMethod { [HarmonyTargetMethod] get; } = AccessTools.Method("AlienRace.HarmonyPatches:TextureAtlasSameRace");

		public static bool AlienRacesDoneWithPatching => Harmony.HasAnyPatches("rimworld.erdelf.alien_race.main");

		[HarmonyPrepare]
		public static bool Prepare() => TargetMethod != null && AlienRacesDoneWithPatching;

		[HarmonyTranspiler]
		public static CodeInstructions Transpiler(CodeInstructions codes)
		{
			try
			{
				return new CodeMatcher(codes)
					.MatchEndForward(
					  new(c => c.LoadsConstant()),
					  new(c => c.IsLdloc()),
					  new(c => c.operand is FieldInfo field && field.Name == "atlasScale"),
					  new(c => c.opcode == OpCodes.Mul))
					.ThrowIfInvalid($"HD Pawn Rendering's Transpiler on {TargetMethod.FullDescription()} failed.")
					.Advance(1)
					.Insert(
					 new(OpCodes.Pop),
					 new(OpCodes.Dup))
					.InstructionEnumeration();
			}
			catch (Exception ex)
			{
				Log.Error($"{ex}");
				return codes;
			}
		}
	}

	[HarmonyPatch]
	public static class HarmonyPatches_TextureAtlasSameRace_FuckYourLINQ
	{
		private static Type? _nestedType = HarmonyPatches_TextureAtlasSameRace.TargetMethod?.GetMethodBody().LocalVariables.First().LocalType;

		public static MethodInfo TargetMethod { [HarmonyTargetMethod] get; }
			= AccessTools.FirstMethod(_nestedType, m
				=> m 
				== (PatchProcessor.ReadMethodBody(HarmonyPatches_TextureAtlasSameRace.TargetMethod).First(instruction
					=> instruction.Key == OpCodes.Ldftn
					&& instruction.Value is MethodInfo method
					&& method.DeclaringType == _nestedType).Value as MethodInfo));

		[HarmonyPrepare]
		public static bool Prepare() => HarmonyPatches_TextureAtlasSameRace.Prepare();

		[HarmonyTranspiler]
		public static CodeInstructions Transpiler(CodeInstructions codes)
		{
			try
			{
				return new CodeMatcher(codes)
					.MatchEndForward(
					  new(c => c.IsLdarg()),
					  new(c => c.operand is FieldInfo field && field.Name == "atlasScale"))
					.ThrowIfInvalid($"HD Pawn Rendering's Transpiler on {TargetMethod?.FullDescription() ?? $"some nested method within {HarmonyPatches_TextureAtlasSameRace.TargetMethod?.FullDescription()}"} failed.")
					.Advance(1)
					.Insert(
					 new(OpCodes.Pop),
					 new(OpCodes.Dup))
					.InstructionEnumeration();
			}
			catch (Exception ex)
			{
				Log.Error($"{ex}");
				return codes;
			}
		}
	}
}