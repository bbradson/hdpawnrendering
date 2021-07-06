using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;
using Verse;

namespace Fish
{
    /*public static class PawnTextureAtlas
    {
        //Comments further down
        public static IEnumerable<CodeInstruction> FishPawnRenderTranspiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            var codes = instructions.ToList();
            for (int i = 0; i < codes.Count; i++)
            {
                if (CodesToChange(codes, i))
                {
                    codes[i].operand = 0x2000;
                    yield return codes[i];
                    codes[i + 1].operand = 0x2000;
                    yield return codes[i + 1];
                    i += 1;
                }
                else
                {
                    yield return codes[i];
                }
            }
        }

        public static bool CodesToChange(List<CodeInstruction> codes, int i)
        {
            return i < codes.Count - 1 &&
                   codes[i].opcode == OpCodes.Ldc_I4 && codes[i].operand is int m && m == 0x800 &&
                   codes[i + 1].opcode == OpCodes.Ldc_I4 && codes[i + 1].operand is int n && n == 0x800;
        }
    }*/

    public static class PawnTextureAtlas
    {
        //Comments further down
        public static IEnumerable<CodeInstruction> FishPawnRenderTranspiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            var codes = instructions.ToList();
            for (int i = 0; i < codes.Count; i++)
            {
                if (CodesToChange(codes, i))
                {
                    codes[i].operand = 0x2000;
                    yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(FixedTexture), nameof(FixedTexture.Create)));
                    i += 5;
                }
                else
                {
                    yield return codes[i];
                }
            }
        }

        public static bool CodesToChange(List<CodeInstruction> codes, int i)
        {
            return i < codes.Count - 1 &&
                   codes[i].opcode == OpCodes.Ldc_I4 && codes[i].operand is int m && m == 0x800 &&
                   codes[i + 1].opcode == OpCodes.Ldc_I4 && codes[i + 1].operand is int n && n == 0x800 &&
                   codes[i + 2].opcode == OpCodes.Ldc_I4_S &&
                   codes[i + 3].opcode == OpCodes.Ldc_I4_0 &&
                   codes[i + 4].opcode == OpCodes.Ldc_I4_0 &&
                   codes[i + 5].opcode == OpCodes.Newobj;
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
            List<Rect> list = new List<Rect>();
            for (int i = 0; i < 2048; i += 128)
            {
                for (int j = 0; j < 2048; j += 128)
                {
                    list.Add(new Rect((float)i / 2048f, (float)j / 2048f, 0.0625f, 0.0625f));
                }
            }
            while (list.Count >= 8)
            {
                PawnTextureAtlasFrameSet pawnTextureAtlasFrameSet = new PawnTextureAtlasFrameSet();
                pawnTextureAtlasFrameSet.uvRects = new Rect[]
                {
                    list.Pop<Rect>(),
                    list.Pop<Rect>(),
                    list.Pop<Rect>(),
                    list.Pop<Rect>(),
                    list.Pop<Rect>(),
                    list.Pop<Rect>(),
                    list.Pop<Rect>(),
                    list.Pop<Rect>()
                };
                pawnTextureAtlasFrameSet.meshes = (from u in pawnTextureAtlasFrameSet.uvRects
                                                   select TextureAtlasHelper.CreateMeshForUV(u, 1f)).ToArray<Mesh>();
                pawnTextureAtlasFrameSet.atlas = this.texture;
                this.freeFrameSets.Add(pawnTextureAtlasFrameSet);
            }
        }

        private RenderTexture texture;

        private Dictionary<Pawn, PawnTextureAtlasFrameSet> frameAssignments = new Dictionary<Pawn, PawnTextureAtlasFrameSet>();

        private List<PawnTextureAtlasFrameSet> freeFrameSets = new List<PawnTextureAtlasFrameSet>();

        private static List<Pawn> tmpPawnsToFree = new List<Pawn>();
    }*/
}
