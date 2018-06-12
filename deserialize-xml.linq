<Query Kind="Program" />

void Main()
{
	var schema = @"C:\Code\innovation\DrivingTest\ProNet\ProNet\ProNet.xsd";
	var xml = @"C:\Code\innovation\DrivingTest\ProNet\ProNet.Test\ProNet.xml";
  
  	var xmlDeserializer = new System.Xml.Serialization.XmlSerializer(schema);
	var programmers = xmlDeserializer.Deserialize(xml);
	
	programmers.Dump();
}

public class Recommendation
{
	private readonly string _recommendation;

	public Recommendation(string recommendation)
	{
		_recommendation = recommendation;
	}
}

public class Recommendations
{
	private readonly Recommendation[] _recommendations;
	
	public Recommendations(Recommendation[] recommendations)
	{
		_recommendations = recommendations;
	}
}

public class Skill
{
	private readonly string _Skill;

	public Skill(string Skill)
	{
		_Skill = Skill;
	}
}

public class Skills
{
	private readonly Skill[] _skills;
	
	public Skills(Skill[] skills)
	{
		_Skills = skills;
	}
}

public class Programmer
{
	private readonly string _Programmer;

	public Programmer(string Programmer)
	{
		_Programmer = Programmer;
	}
}

public class Programmers
{
	private readonly Programmer[] _Programmers;
	
	public Programmers(Programmer[] programmers)
	{
		_Programmers = programmers;
	}
}