using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace Fish
{
    public class fishsettings : ModSettings
    {
        public void DoSettingsWindowContents(Rect inRect)
        {
            const int increment = 64;

            if (!initialized)
            {
                Initialize();
                initialized = true;
            }

            Rect rekt = new(15f, 75f, inRect.width - 30f, inRect.height);
            Listing_Standard ls = new();
            ls.Begin(rekt);

            var textFieldRect = ls.GetRect(Text.LineHeight);
            if (Mouse.IsOver(textFieldRect))
            {
                TooltipHandler.TipRegion(textFieldRect, fishResInfo);
            }
            var texRes = TextureResolution;

            var texResString = fishResButton;
            textFieldRect.width = texResString.GetWidthCached();
            Widgets.Label(textFieldRect, texResString);

            textFieldRect.x += textFieldRect.width + 12f;
            textFieldRect.width = 42f;

            if (Widgets.ButtonText(textFieldRect, "-" + increment, true, true, true))
            {
                SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
                texRes -= increment * GenUI.CurrentAdjustmentMultiplier();
                if (texRes < increment)
                {
                    texRes = increment;
                }
            }
            textFieldRect.x += textFieldRect.width + 12f;
            Widgets.Label(textFieldRect, texRes.ToString());
            textFieldRect.x += textFieldRect.width + 2f;
            if (Widgets.ButtonText(textFieldRect, "+" + increment, true, true, true))
            {
                SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
                texRes += increment * GenUI.CurrentAdjustmentMultiplier();
                if (texRes < increment)
                {
                    texRes = increment;
                }
            }
            TextureResolution = texRes;
            ls.Gap(ls.verticalSpacing);

            InstantCheckboxLabeled(ls.GetRect(Text.LineHeight), fishMipsButton, ref FixedTexture.mMenabled, fishMipsInfo);
            ls.Gap(ls.verticalSpacing);

            ls.Gap(30f);

            ls.Label(fishExperimental);
            InstantCheckboxLabeled(ls.GetRect(Text.LineHeight), fishAAButton, ref FixedTexture.aAenabled, fishAAInfo);
            ls.Gap(ls.verticalSpacing);

            if (FixedTexture.aAenabled)
            {
                var sliderRect = ls.GetRect(48f);
                Widgets.Label(sliderRect.TopHalf(), fishLevelButton);
                FixedTexture.aAlevel = (int)Widgets.HorizontalSlider(sliderRect.BottomHalf(), FixedTexture.aAlevel, 2f, 16f, label: FixedTexture.aAlevel.ToString(), roundTo: 1f);
                if (Mouse.IsOver(sliderRect))
                {
                    TooltipHandler.TipRegion(sliderRect, fishLevelInfo);
                }
            }

            ls.End();
            base.Write();
        }

        public void InstantCheckboxLabeled(Rect rect, string label, ref bool checkOn, TipSignal tooltip)
        {
            float lineHeight = Text.LineHeight;
            if (Mouse.IsOver(rect))
            {
                Widgets.DrawHighlight(rect);
                TooltipHandler.TipRegion(rect, tooltip);
            }
            Widgets.CheckboxLabeled(rect, label, ref checkOn);
        }

        public int TextureResolution
        {
            get { return FixedTexture.textureResolution / 16; }
            set { FixedTexture.textureResolution = value * 16; }
        }

        public void Initialize()
        {
            fishResButton = "HDPR.ResButton".Translate();
            fishResInfo = new("HDPR.ResInfo".Translate(), 0f);
            fishMipsButton = "HDPR.MipsButton".Translate();
            fishMipsInfo = new("HDPR.MipsInfo".Translate(), 0f);
            fishExperimental = "HDPR.Experimental".Translate();
            fishAAButton = "HDPR.AAButton".Translate();
            fishAAInfo = new("HDPR.AAInfo".Translate(), 0f);
            fishLevelButton = "HDPR.AALevelButton".Translate();
            fishLevelInfo = new("HDPR.AALevelInfo".Translate(), 0f);
        }

        private bool initialized = false;
        private TaggedString fishResButton;
        private TipSignal fishResInfo;
        private string fishMipsButton;
        private TipSignal fishMipsInfo;
        private string fishExperimental;
        private string fishAAButton;
        private TipSignal fishAAInfo;
        private string fishLevelButton;
        private TipSignal fishLevelInfo;

        public override void ExposeData()
        {
            Scribe_Values.Look<bool>(ref FixedTexture.aAenabled, "AAenabled", false);
            Scribe_Values.Look<bool>(ref FixedTexture.mMenabled, "MMenabled", true);
            Scribe_Values.Look<int>(ref FixedTexture.aAlevel, "AAlevel", 4);
            Scribe_Values.Look<int>(ref FixedTexture.textureResolution, "textureResolution", 8192);
        }
    }
}
