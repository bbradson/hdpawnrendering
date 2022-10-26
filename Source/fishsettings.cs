// Copyright (c) 2022 bradson
// This Source Code Form is subject to the terms of the MIT license.
// If a copy of the license was not distributed with this file,
// You can obtain one at https://opensource.org/licenses/MIT/.

using Verse.Sound;

namespace Fish;

[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "regret")]
public class fishsettings : ModSettings
{
	public static void DoSettingsWindowContents(Rect inRect)
	{
		//const int INCREMENT = 64;

		Rect rekt = new(15f, 75f, inRect.width - 30f, inRect.height);
		Listing_Standard ls = new();
		ls.Begin(rekt);

		TextureQualitySlider(ls);
		ls.Gap(ls.verticalSpacing);

		InstantCheckboxLabeled(ls.GetRect(Text.LineHeight), Strings.MipsButton, ref FixedTexture.MipMapsEnabled, TipSignals.MipsInfo);
		/*ls.Gap(ls.verticalSpacing);

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
				TooltipHandler.TipRegion(sliderRect, TipSignals.LevelInfo);
		}*/

		ls.End();
		Fish.Settings!.Write();
	}

	private static void TextureQualitySlider(Listing_Standard ls)
	{
		var previousAlignment = Text.Anchor;
		Text.Anchor = TextAnchor.MiddleRight;
		var labelRect = new Rect() { x = ls.curX, y = ls.curY, width = ls.ColumnWidth, height = Text.CalcHeight("384", ls.ColumnWidth) };
		Widgets.Label(labelRect, $"{TextureResolution}");
		Text.Anchor = previousAlignment;

		if (Mouse.IsOver(labelRect))
			TooltipHandler.TipRegion(labelRect, TipSignals.ResInfo);

		ls.Label(Strings.ResButton);
		var sliderResult = Mathf.RoundToInt(ls.Slider(TextureResolutionFactor, 10, 22));
		if (sliderResult != TextureResolutionFactor)
		{
			TextureResolutionFactor = sliderResult;
			var powerOfTwo = Mathf.RoundToInt((float)Math.Pow(2, Mathf.FloorToInt((sliderResult / 2) + 0.01f)));
			TextureResolution = powerOfTwo + ((sliderResult % 2) != 0 ? Mathf.RoundToInt(powerOfTwo / 2) : 0);
		}
	}

	private static int? _textureResolutionFactor;

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

	public static int TextureResolutionFactor
	{
		get => _textureResolutionFactor ??= GetInitialFactor();
		set => _textureResolutionFactor = value;
	}

	private static int GetInitialFactor()
	{
		var isPowerOfTwo = Mathf.IsPowerOfTwo(TextureResolution);
		var log = Mathf.RoundToInt(Mathf.Log(!isPowerOfTwo ? TextureResolution * 2 / 3 : TextureResolution, 2)) * 2;
		return !isPowerOfTwo ? log + 1 : log;
	}

	public override void ExposeData()
		=> FixedTexture.Scribe();
}
