using UnityEngine;

namespace Fish
{
    public static class FixedTexture
    {
        public static RenderTexture Create()
        {
            RenderTexture tex = new(textureResolution, textureResolution, 24, RenderTextureFormat.ARGB32);
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
        }

        public const float mMbias = -0.7f;
        public static int textureResolution = 8192;
        public static bool aAenabled = false;
        public static bool mMenabled = true;
        public static int aAlevel = 4;
    }
}
