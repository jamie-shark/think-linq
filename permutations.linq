<Query Kind="Program" />

void Main()
{
	var input = new[] {"a", "b", "c", "d", "e"};
	
	var bitOperationPermutations = new BitOperataionPermutations().Permutations(input, 3);
	var selectManyPermutations = new SelectManyPermutations().Permutations(input, 3);
	
	(bitOperationPermutations == selectManyPermutations).Dump();
}

public interface IPermutations
{
	IEnumerable<IEnumerable<T>> Permutations<T>(IEnumerable<T> set, int size) where T : IEquatable<T>;
}

public class BitOperataionPermutations : IPermutations
{
	public IEnumerable<IEnumerable<T>> Permutations<T>(IEnumerable<T> set, int size) where T : IEquatable<T>
	{
		var list = set.ToList();
		return Enumerable.Range(0, 1 << list.Count)
			.Select(m =>
				Enumerable.Range (0, list.Count)
					.Where (i => ((m & (1 << i)) != 0))
					.Select (i => list[i]))
			.Where(x => x.Count() == size);
	}
}

public class SelectManyPermutations : IPermutations
{
	public IEnumerable<IEnumerable<T>> Permutations<T>(IEnumerable<T> set, int size) where T : IEquatable<T>
	{
		return set.SelectMany(_ => set, (fst, snd) => new { fst, snd })
			.Where(x => !x.fst.Equals(x.snd))
			.Select(x => new[] { x.fst, x.snd }.AsEnumerable());
	}
}