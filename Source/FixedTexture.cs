// Copyright (c) 2022 bradson
// This Source Code Form is subject to the terms of the MIT license.
// If a copy of the license was not distributed with this file,
// You can obtain one at https://opensource.org/licenses/MIT/.

namespace Fish;

public static class FixedTexture
{
	public static RenderTexture Create()
	{
		RenderTexture tex = new(TextureResolution, TextureResolution, 24, RenderTextureFormat.ARGB32);
		/*if (AAenabled)
		{
			tex.antiAliasing = AAlevel;
		}*/
		if (MipMapsEnabled)
		{
			tex.useMipMap = true;
			tex.mipMapBias = MipMapBias;
		}
		return tex;
	}

	public static void Scribe()
	{
		Scribe_Values.Look(ref AAenabled, "AAenabled", false);
		Scribe_Values.Look(ref MipMapsEnabled, "MMenabled", true);
		Scribe_Values.Look(ref AAlevel, "AAlevel", 4);
		Scribe_Values.Look(ref TextureResolution, "textureResolution", 6144);
	}

	public static float MipMapBias = -0.7f;
	public static int TextureResolution = 6144;
	public static bool AAenabled = false;
	public static bool MipMapsEnabled = true;
	public static int AAlevel = 4;
}
