// Copyright (c) 2022 bradson
// This Source Code Form is subject to the terms of the MIT license.
// If a copy of the license was not distributed with this file,
// You can obtain one at https://opensource.org/licenses/MIT/.

global using System;
global using System.Collections.Generic;
global using UnityEngine;
global using Verse;
global using HarmonyLib;

namespace Fish;

public class Fish : Mod
{
	public static Fish? Mod { get; private set; }
	public static fishsettings? Settings { get; private set; }

	public Fish(ModContentPack content) : base(content)
	{
		Harmony harmony = new("fish");

		if (Type.GetType("GraphicSetter.GraphicsPatches+PawnTextureAtlasCtorPatch, GraphicSetter") is { } graphicsSetterType)
			harmony.Unpatch(AccessTools.Constructor(typeof(Verse.PawnTextureAtlas)), AccessTools.Method(graphicsSetterType, "Transpiler"));

		harmony.Patch(AccessTools.Constructor(typeof(Verse.PawnTextureAtlas)), transpiler: new(((Delegate)PawnTextureAtlas.FishPawnRenderTranspiler).Method, Priority.First));

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