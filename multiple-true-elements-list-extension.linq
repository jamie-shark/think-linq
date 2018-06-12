<Query Kind="Program" />

void Main()
{
	List<bool> flags = new List<bool> { true, true, false };
	flags.HasMultipleTrueElements().Dump();
}

internal static class ListExtensionMethods
{
	internal static bool HasMultipleTrueElements(this List<bool> list)
	{
		return list.Where(f => f).Skip(1).Any();
	}
}
