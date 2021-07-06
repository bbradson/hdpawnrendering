using Verse;
using HarmonyLib;
using UnityEngine;

namespace Fish
{
    public class Fish : Mod
    {
        public static Fish mod;
        public static fishsettings settings;

        public Fish(ModContentPack content) : base(content)
        {
            //Harmony.DEBUG = false;
            //FileLog.Reset();

            Harmony ha = new Harmony("fish");
            ha.Patch(original: AccessTools.Constructor(typeof(Verse.PawnTextureAtlas)), transpiler: new HarmonyMethod(typeof(PawnTextureAtlas), nameof(PawnTextureAtlas.FishPawnRenderTranspiler)));

            settings = GetSettings<fishsettings>();
            mod = this;
        }

        public override string SettingsCategory()
        {
            if (!initialized)
            {
                fishName = "HDPR.Name".Translate();
                initialized = true;
            }
            return fishName;
        }

        private bool initialized = false;
        private string fishName;

        public override void DoSettingsWindowContents(Rect inRect)
        {
            base.DoSettingsWindowContents(inRect);
            settings.DoSettingsWindowContents(inRect);
        }
    }
}
