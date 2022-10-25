// Copyright (c) 2022 bradson
// This Source Code Form is subject to the terms of the MIT license.
// If a copy of the license was not distributed with this file,
// You can obtain one at https://opensource.org/licenses/MIT/.

namespace Fish;

[StaticConstructorOnStartup]
public static class StaticConstructor
{
	static StaticConstructor()
	{
		// Disable disableCaching (and in turn re-enable it)
		if (Type.GetType("VFECore.VFEGlobal, VFECore")?.GetField("settings")?.GetValue(null) is { } vEFsettings)
			Type.GetType("VFECore.VFEGlobalSettings, VFECore")?.GetField("disableCaching")?.SetValue(vEFsettings, false);
	}
}