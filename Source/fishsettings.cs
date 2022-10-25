// Copyright (c) 2022 bradson
// This Source Code Form is subject to the terms of the MIT license.
// If a copy of the license was not distributed with this file,
// You can obtain one at https://opensource.org/licenses/MIT/.

using RimWorld;
using Verse.Sound;

namespace Fish;

[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "regret")]
public class fishsettings : ModSettings
{
	public static void DoSettingsWindowContents(Rect inRect)
	{
		const int INCREMENT = 64;

		Rect rekt = new(15f, 75f, inRect.width - 30f, inRect.height);
		Listing_Standard ls = new();
		ls.Begin(rekt);

		var textFieldRect = ls.GetRect(Text.LineHeight);
		if (Mouse.IsOver(textFieldRect))
		{
			TooltipHandler.TipRegion(textFieldRect, TipSignals.ResInfo);
		}
		var texRes = TextureResolution;

		textFieldRect.width = Strings.ResButton.GetWidthCached();
		Widgets.Label(textFieldRect, Strings.ResButton);

		textFieldRect.x += textFieldRect.width + 12f;
		textFieldRect.width = 42f;

		if (Widgets.ButtonText(textFieldRect, $"-{INCREMENT}", true, true, true))
		{
			SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
			texRes -= INCREMENT * GenUI.CurrentAdjustmentMultiplier();
			if (texRes < INCREMENT)
			{
				texRes = INCREMENT;
			}
		}
		textFieldRect.x += textFieldRect.width + 12f;
		Widgets.Label(textFieldRect, texRes.ToString());
		textFieldRect.x += textFieldRect.width + 2f;
		if (Widgets.ButtonText(textFieldRect, $"+{INCREMENT}", true, true, true))
		{
			SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
			texRes += INCREMENT * GenUI.CurrentAdjustmentMultiplier();
			if (texRes < INCREMENT)
			{
				texRes = INCREMENT;
			}
		}
		TextureResolution = texRes;
		ls.Gap(ls.verticalSpacing);

		InstantCheckboxLabeled(ls.GetRect(Text.LineHeight), Strings.MipsButton, ref FixedTexture.MipMapsEnabled, TipSignals.MipsInfo);
		ls.Gap(ls.verticalSpacing);

		ls.Gap(30f);

		ls.Label(Strings.Experimental);
		InstantCheckboxLabeled(ls.GetRect(Text.LineHeight), Strings.AAButton, ref FixedTexture.AAenabled, TipSignals.AAInfo);
		ls.Gap(ls.verticalSpacing);

		if (FixedTexture.AAenabled)
		{
			var sliderRect = ls.GetRect(48f);
			Widgets.Label(sliderRect.TopHalf(), Strings.LevelButton);
			FixedTexture.AAlevel = (int)Widgets.HorizontalSlider(sliderRect.BottomHalf(), FixedTexture.AAlevel, 2f, 16f, label: FixedTexture.AAlevel.ToString(), roundTo: 1f);
			if (Mouse.IsOver(sliderRect))
			{
				TooltipHandler.TipRegion(sliderRect, TipSignals.LevelInfo);
			}
		}

		ls.End();
		Fish.Settings!.Write();
	}

	public static void InstantCheckboxLabeled(Rect rect, string label, ref bool checkOn, TipSignal tooltip)
	{
		if (Mouse.IsOver(rect))
		{
			Widgets.DrawHighlight(rect);
			TooltipHandler.TipRegion(rect, tooltip);
		}
		Widgets.CheckboxLabeled(rect, label, ref checkOn);
	}

	public static int TextureResolution
	{
		get => FixedTexture.TextureResolution / 16;
		set => FixedTexture.TextureResolution = value * 16;
	}

	public override void ExposeData()
		=> FixedTexture.Scribe();
}
