<Query Kind="Program">
  <Reference>&lt;RuntimeDirectory&gt;\System.Threading.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Threading.Tasks.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Threading.Thread.dll</Reference>
  <Namespace>System</Namespace>
  <Namespace>System.Threading</Namespace>
</Query>

void Main()
{
	var links = new List<Link>
	{
		new Link { Rel = "a b", Uri = "A" },
		new Link { Rel = "b", Uri = "B" },
	};
	GetLinkForRel("a", links.AsQueryable()).Dump();
}

Link GetLinkForRel(string rel, IQueryable<Link> _links)
{
  	return _links.Aggregate((matched, link) =>
		matched == null && link.Rel.Split(' ').Contains(rel) 
			? link
			: (Link)null);
}

class Link
{
	public string Rel { get; set; }
	public string Uri { get; set; }
}
