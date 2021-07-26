using HarmonyLib;
using Verse;

namespace Fish
{
    public class Fish : Mod
    {
        public static Fish mod;
        public static fishsettings settings;

        public Fish(ModContentPack content) : base(content)
        {
            Harmony ha = new("fish");
            ha.Patch(AccessTools.Constructor(typeof(Verse.PawnTextureAtlas)), transpiler: new(typeof(PawnTextureAtlas).GetMethod(nameof(PawnTextureAtlas.FishPawnRenderTranspiler)), Priority.First));

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

        public override void DoSettingsWindowContents(UnityEngine.Rect inRect)
        {
            base.DoSettingsWindowContents(inRect);
            settings.DoSettingsWindowContents(inRect);
        }
    }
}
