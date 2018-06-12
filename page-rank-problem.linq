<Query Kind="Program" />

void Main()
{
	var programmerB = new Programmer("b", new List<string>{"a"});
	var programmerA = new Programmer("a", new List<string>{"b"});
	var programmers = new List<Programmer>{programmerA, programmerB};
	
	for (var i = 0; i < 10; ++i)
	{
		new RankCalculator(programmers, i).PageRank(programmerA).Dump();
		new RankCalculator(programmers, i).PageRank(programmerB).Dump();
	}
}

public class Programmer
{
	public string Id { get; private set; }
	public List<string> Recommendations { get; private set; }

	public Programmer(string id, List<string> recommendations)
	{
		Id = id;
		Recommendations = recommendations;
	}
}

public class RankCalculator
{
	private int _settleLimit;
	private int _iteration = 0;
	private decimal _rank = 0m;
	private List<Programmer> _programmers;
	
	public RankCalculator(List<Programmer> programmers, int settleLimit)
	{
		_programmers = programmers;
		_settleLimit = settleLimit;
	}
	
	public decimal PageRank(Programmer programmer)
	{
		var damp = 0.85m;
		while (++_iteration < _settleLimit)
			_rank = (1 - damp) + damp * (_programmers.Where(p => programmer.Recommendations.Contains(p.Id)).Select(r => PageRank(r) / Count(r)).Sum());
			
		return _rank;
	}
	
	private int Count(Programmer programmer)
	{
		return programmer.Recommendations.Count();
	}
}
