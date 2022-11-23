// Copyright (c) 2022 bradson
// This Source Code Form is subject to the terms of the MIT license.
// If a copy of the license was not distributed with this file,
// You can obtain one at https://opensource.org/licenses/MIT/.

global using System;
global using System.Reflection.Emit;
global using HarmonyLib;
global using UnityEngine;
global using Verse;
global using CodeInstructions = System.Collections.Generic.IEnumerable<HarmonyLib.CodeInstruction>;

namespace Fish;

public class Fish : Mod
{
	public static Fish? Mod { get; private set; }
	public static fishsettings? Settings { get; private set; }
	public static Harmony Harmony { get; } = new("fish");

	public Fish(ModContentPack content) : base(content)
	{
		if (Type.GetType("GraphicSetter.GraphicsPatches+PawnTextureAtlasCtorPatch, GraphicSetter") is { } graphicsSetterType)
			Harmony.Unpatch(AccessTools.Constructor(typeof(Verse.PawnTextureAtlas)), AccessTools.Method(graphicsSetterType, "Transpiler"));

		Harmony.PatchAll(typeof(Fish).Assembly);

		Settings = GetSettings<fishsettings>();
		Mod = this;
	}

	public override string SettingsCategory() => Strings.ModName;

	public override void DoSettingsWindowContents(Rect inRect)
	{
		base.DoSettingsWindowContents(inRect);
		fishsettings.DoSettingsWindowContents(inRect);
	}
}