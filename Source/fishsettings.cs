using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace Fish
{
    public class fishsettings : ModSettings
    {
        public void DoSettingsWindowContents(Rect inRect)
        {
            if (!initialized)
            {
                Initialize();
                initialized = true;
            }

            Rect rekt = new Rect(15f, 75f, inRect.width - 30f, inRect.height);
            Listing_Standard ls = new Listing_Standard();
            ls.Begin(rekt);

            var textFieldRect = ls.GetRect(Text.LineHeight);
            if (Mouse.IsOver(textFieldRect))
            {
                TooltipHandler.TipRegion(textFieldRect, fishResInfo);
            }
            var texRes = TextureResolution;
            //Widgets.TextFieldNumericLabeled<int>(textFieldRect, "Pawn Texture Resolution: ", ref texRes, ref buffer, 64f, 4096f); //This text field is fucking trash.

            TaggedString texResString = fishResButton;
            textFieldRect.width = texResString.GetWidthCached();
            Widgets.Label(textFieldRect, texResString);

            textFieldRect.x += textFieldRect.width + 12f;
            textFieldRect.width = 42f;

            const int increment = 64;
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
                    TooltipHandler.TipRegion(sliderRect, new TipSignal(fishLevelInfo, 0f));
                }
            }

            ls.End();
            base.Write();
        }

        public void InstantCheckboxLabeled(Rect rect, string label, ref bool checkOn, string tooltip = null)
        {
            float lineHeight = Text.LineHeight;
            if (!tooltip.NullOrEmpty())
            {
                if (Mouse.IsOver(rect))
                {
                    Widgets.DrawHighlight(rect);
                    TooltipHandler.TipRegion(rect, new TipSignal(tooltip, 0f));
                }
            }
            Widgets.CheckboxLabeled(rect, label, ref checkOn, false, null, null, false);
        }

        public int TextureResolution
        {
            get { return FixedTexture.textureResolution / 16; }
            set { FixedTexture.textureResolution = value * 16; }
        }

        public void Initialize()
        {
            fishResButton = "HDPR.ResButton".Translate();
            fishResInfo = new TipSignal("HDPR.ResInfo".Translate(), 0f);
            fishMipsButton = "HDPR.MipsButton".Translate();
            fishMipsInfo = "HDPR.MipsInfo".Translate();
            fishExperimental = "HDPR.Experimental".Translate();
            fishAAButton = "HDPR.AAButton".Translate();
            fishAAInfo = "HDPR.AAInfo".Translate();
            fishLevelButton = "HDPR.AALevelButton".Translate();
            fishLevelInfo = "HDPR.AALevelInfo".Translate();
        }

        private bool initialized = false;
        private string fishResButton;
        private TipSignal fishResInfo;
        private string fishMipsButton;
        private string fishMipsInfo;
        private string fishExperimental;
        private string fishAAButton;
        private string fishAAInfo;
        private string fishLevelButton;
        private string fishLevelInfo;

        public static string buffer = "512";
        public static FloatRange aArange = new FloatRange(2f, 16f);

        public override void ExposeData()
        {
            Scribe_Values.Look<bool>(ref FixedTexture.aAenabled, "AAenabled", false);
            Scribe_Values.Look<bool>(ref FixedTexture.mMenabled, "MMenabled", true);
            Scribe_Values.Look<int>(ref FixedTexture.aAlevel, "AAlevel", 4);
            Scribe_Values.Look<int>(ref FixedTexture.textureResolution, "textureResolution", 8192);
        }
    }
}
