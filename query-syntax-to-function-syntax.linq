<Query Kind="Program" />

void Main()
{
	var updatedProperties =
      from property in new TableTypeThing().GetProperties().AsQueryable()
           let propertyValue = Reflection.GetProperty(new TableTypeThing(), property.Name, new object())
           where propertyValue != null
               let defaultPropertyValue = Reflection.GetProperty(new TableTypeThing(), property.Name, Reflection.CreateInstance(new TableTypeThing()))
               where defaultPropertyValue == null || !defaultPropertyValue.Equals(new object())
                   select new Tuple<string, object>(property.Name, propertyValue);
						
	updatedProperties.Dump();
}

class Reflection
{
	public static object GetProperty(TableTypeThing tableType, string name, object instance)
	{
		return new object();
	}
	
	public static object CreateInstance(TableTypeThing tableType)
	{
		return new object();
	}	
}

class TableTypeThing
{
	public IEnumerable<Property> GetProperties()
	{
		return new List<Property> { new Property { Name = "Thing" }};	
	}
}

class Property
{
	public string Name { get; set; }
}

// Define other methods and classes here
