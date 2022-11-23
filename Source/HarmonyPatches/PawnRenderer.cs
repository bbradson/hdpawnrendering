// Copyright (c) 2022 bradson
// This Source Code Form is subject to the terms of the MIT license.
// If a copy of the license was not distributed with this file,
// You can obtain one at https://opensource.org/licenses/MIT/.

#if V1_4
namespace Fish;

[HarmonyPatch(typeof(Verse.PawnRenderer), nameof(Verse.PawnRenderer.RenderPawnAt))]
public static class PawnRenderer
{
	[HarmonyTranspiler]
	public static CodeInstructions Transpiler(CodeInstructions codes)
	{
		var success = false;

		foreach (var code in codes)
		{
			yield return code;

			if (code.Calls(AccessTools.PropertyGetter(typeof(CameraDriver), nameof(CameraDriver.ZoomRootSize))))
			{
				yield return new(OpCodes.Pop);
				yield return new(OpCodes.Ldc_R4, float.PositiveInfinity);
				success = true;
			}
		}

		if (!success)
			Log.Error("Transpiler on Verse.PawnRenderer.RenderPawnAt failed");
	}
}
#endif