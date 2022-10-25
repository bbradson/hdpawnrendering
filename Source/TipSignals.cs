// Copyright (c) 2022 bradson
// This Source Code Form is subject to the terms of the MIT license.
// If a copy of the license was not distributed with this file,
// You can obtain one at https://opensource.org/licenses/MIT/.

namespace Fish;
public static class TipSignals
{
	public static TipSignal ResInfo { get; } = new(Strings.ResInfo, 0f);
	public static TipSignal MipsInfo { get; } = new(Strings.MipsInfo, 0f);
	public static TipSignal AAInfo { get; } = new(Strings.AAInfo, 0f);
	public static TipSignal LevelInfo { get; } = new(Strings.AALevelInfo, 0f);
	static TipSignals() { } // prevent beforefieldinit
}