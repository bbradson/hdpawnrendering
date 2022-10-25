// Copyright (c) 2022 bradson
// This Source Code Form is subject to the terms of the MIT license.
// If a copy of the license was not distributed with this file,
// You can obtain one at https://opensource.org/licenses/MIT/.

namespace Fish;
public static class Strings
{
	public static string ModName { get; } = "HDPR.Name";
	public static string ResButton { get; } = "HDPR.ResButton";
	public static string MipsButton { get; } = "HDPR.MipsButton";
	public static string Experimental { get; } = "HDPR.Experimental";
	public static string AAButton { get; } = "HDPR.AAButton";
	public static string LevelButton { get; } = "HDPR.AALevelButton";
	public static string ResInfo { get; } = "HDPR.ResInfo";
	public static string MipsInfo { get; } = "HDPR.MipsInfo";
	public static string AAInfo { get; } = "HDPR.AAInfo";
	public static string AALevelInfo { get; } = "HDPR.AALevelInfo";

	static Strings()
		=> TranslateAllFields(typeof(Strings));

	private static void TranslateAllFields(Type type)
	{
		foreach (var field in type.GetFields(AccessTools.allDeclared))
		{
			if (field.FieldType == typeof(string))
				field.SetValue(null, ((string)field.GetValue(null)).TranslateSimple());
		}
	}
}