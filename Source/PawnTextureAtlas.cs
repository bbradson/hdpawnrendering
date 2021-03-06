using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Verse;
using static System.Reflection.Emit.OpCodes;

namespace Fish
{
    public static class PawnTextureAtlas
    {
        public static IEnumerable<CodeInstruction> FishPawnRenderTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            var codes = instructions.ToArray();
            var success = false;
            for (int i = 0; i < codes.Length; i++)
            {
                if (CodesToChange(codes, i))
                {
                    yield return new(Call, typeof(FixedTexture).GetMethod(nameof(FixedTexture.Create)));
                    i += 5;
                    success = true;
                }
                else
                {
                    yield return codes[i];
                }
            }
            if (!success)
            {
                Log.Error("HD Pawn Rendering failed to apply its patch on Verse.PawnTextureAtlas. This generally means the mod isn't working due to some incompatibility or whatever. Hugslib might have info on what else could be patching the method and breaking the mod in the process.");
            }
        }

        public static bool CodesToChange(CodeInstruction[] codes, int i)
        {
            return i + 5 < codes.Length &&
                   codes[i].opcode == Ldc_I4 && codes[i].operand is int m && m == 0x800 &&
                   codes[i + 1].opcode == Ldc_I4 && codes[i + 1].operand is int n && n == 0x800 &&
                   codes[i + 2].opcode == Ldc_I4_S &&
                   codes[i + 3].opcode == Ldc_I4_0 &&
                   codes[i + 4].opcode == Ldc_I4_0 &&
                   codes[i + 5].opcode == Newobj;
        }
    }

    /// <summary>
    /// Above is the transpiler, below is what the result should look like after transpiling.
    /// </summary>
    /*class OGPawnTextureAtlas
    {
        public OGPawnTextureAtlas()
        {
            this.texture = FixedTexture.Create();
            List<UnityEngine.Rect> list = new();
            for (int i = 0; i < 2048; i += 128)
            {
                for (int j = 0; j < 2048; j += 128)
                {
                    list.Add(new((float)i / 2048f, (float)j / 2048f, 0.0625f, 0.0625f));
                }
            }
            while (list.Count >= 8)
            {
                PawnTextureAtlasFrameSet pawnTextureAtlasFrameSet = new();
                pawnTextureAtlasFrameSet.uvRects = new[]
                {
                    list.Pop(),
                    list.Pop(),
                    list.Pop(),
                    list.Pop(),
                    list.Pop(),
                    list.Pop(),
                    list.Pop(),
                    list.Pop()
                };
                pawnTextureAtlasFrameSet.meshes = (from u in pawnTextureAtlasFrameSet.uvRects
                                                   select TextureAtlasHelper.CreateMeshForUV(u, 1f)).ToArray();
                pawnTextureAtlasFrameSet.atlas = this.texture;
                this.freeFrameSets.Add(pawnTextureAtlasFrameSet);
            }
        }

        private UnityEngine.RenderTexture texture;

        private Dictionary<Pawn, PawnTextureAtlasFrameSet> frameAssignments = new();

        private List<PawnTextureAtlasFrameSet> freeFrameSets = new();

        private static List<Pawn> tmpPawnsToFree = new();
    }*/
}
