<Query Kind="Program">
  <Reference>&lt;ProgramFilesX64&gt;\Microsoft SDKs\Azure\.NET SDK\v2.9\bin\plugins\Diagnostics\Newtonsoft.Json.dll</Reference>
  <Reference>C:\Windows\Microsoft.NET\Framework64\v4.0.30319\System.Linq.Parallel.dll</Reference>
  <Reference>C:\Windows\Microsoft.NET\Framework64\v4.0.30319\System.Net.dll</Reference>
  <Reference>C:\Windows\Microsoft.NET\Framework64\v4.0.30319\System.Net.Http.dll</Reference>
  <Reference>C:\Windows\Microsoft.NET\Framework64\v4.0.30319\System.Net.Http.WebRequest.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Threading.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Threading.Tasks.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Threading.Thread.dll</Reference>
  <Namespace>System.Threading</Namespace>
</Query>

void Main()
{
	var team = new[] { 1, 2, 3, 4 };
	new CombinationService().GetCombinations(team, 2).Dump();
	new PermutationService().GetPermutations(team, 3).Dump();
}

public class CombinationService
{
    public IEnumerable<IEnumerable<T>> GetCombinations<T>(IEnumerable<T> set, int size) where T : IEquatable<T>
    {
        if (size == 1)
        {
            return set.Select(item => new[] {item});
        }

        return GetCombinations(set, size - 1).SelectMany(GetSetWithoutCurrentItem(set), AddCombinationToResults);
    }

    private static Func<IEnumerable<T>, IEnumerable<T>> GetSetWithoutCurrentItem<T>(IEnumerable<T> set)
    {
        return item => set.Where(i => !item.Contains(i));
    }

    private static IEnumerable<T> AddCombinationToResults<T>(IEnumerable<T> item, T combination)
    {
        return item.Concat(new[] {combination});
    }
}

public class PermutationService
{
    public IEnumerable<IEnumerable<T>> GetPermutations<T>(IEnumerable<T> set, int size) where T : IEquatable<T>
    {
        return PermutationsFor(set).Where(x => x.Count() == size);
    }

    private static IEnumerable<IEnumerable<T>> PermutationsFor<T>(IEnumerable<T> set)
    {
        var list = set.ToList();
        var permutations = Enumerable.Range(0, 1 << list.Count)
            .Select(item1 =>
                Enumerable.Range(0, list.Count)
                    .Where(item2 => (item1 & (1 << item2)) != 0)
                    .Select(item => list[item])
            );
        return permutations.Select(x => x.AsEnumerable());
    }
}