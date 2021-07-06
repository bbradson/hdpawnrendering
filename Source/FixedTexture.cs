using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace Fish
{
    public class FixedTexture
    {
        public static RenderTexture Create()
        {
            var tex = new RenderTexture(textureResolution, textureResolution, 24, RenderTextureFormat.ARGB32, 0);
            if (aAenabled)
            {
                tex.antiAliasing = aAlevel;
            }
            if (mMenabled)
            {
                tex.useMipMap = true;
                tex.mipMapBias = mMbias;
            }
            return tex;

            /*return new RenderTexture(8192, 8192, 24, RenderTextureFormat.ARGB32, 0)
            {
                antiAliasing = AAlevel,
                mipMapBias = MMbias,
                useMipMap = true
            };*/
        }

        public const float mMbias = -0.7f;
        public static int textureResolution = 8192;
        public static bool aAenabled = false;
        public static bool mMenabled = true;
        public static int aAlevel = 4;
    }
}
