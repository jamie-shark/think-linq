<Query Kind="Program">
  <Reference>C:\Code\main\Product\Fri.Xhl.Domain.Events\bin\Debug\Fri.Xhl.Domain.Events.dll</Reference>
  <Reference>C:\Code\insight-projector\packages\protobuf-net.2.3.2\lib\net40\protobuf-net.dll</Reference>
  <Namespace>Fri.Xhl.Domain.Events</Namespace>
  <Namespace>ProtoBuf</Namespace>
</Query>

void Main()
{
	string byteString1 = "0A1209642FF9289626364F11836FA2D10014EDA71212094AD62E18C6B4114F11A7E7A2D10014DBF91864220D7465737440746573742E636F6D320B0892EE9698CB9EBB31100538D3DC1E40AE9E1B48036204083718046A036672697A0353453182010753453120304853AA06120942B306690F9E5D4811A3A3A2D10014EDACB2061209642FF9289626364F11836FA2D10014EDA7B80602CA060D3139322E3136382E302E313134D2060D4C4F4E2D5452554E4B2D324B38DA060B08AADCDD97CB9EBB311005";
	string byteString2 = "0A1209345762E701230740119B5EA2D2012DA875121209BF7B0084247E7741119EA2A2D2012B01FB18FA01220E746573743240746573742E636F6D2801320B089A97A3D4C9F4BB311005AA0612095082C1A01037AC4C11A336A2D2012DA87AB2061209345762E701230740119B5EA2D2012DA875B80602C20606776B6F756461CA06033A3A31D2060D444556564D57524B53544E3033DA060B0896ECDCD3C9F4BB311005";
	
	string[] events = new[] {byteString1, byteString2};
	
	var count = 0;
	
	foreach (var byteString in events)
	{
		byte[] serialisedEvent = StringToByteArray(byteString);
		NewEvent @event = Deserialize<NewEvent>(serialisedEvent, typeof(NewEvent));
		@event.Dump();
		count.Dump();
		count++;
	}
}

[Serializable]
[ProtoContract]
public class NewEvent
{
	[ProtoMember(1)]
	public Guid Property1 { get; set; }

	[ProtoMember(2)]
	public Guid Property2 { get; set; }	
	
	[ProtoMember(3)]
	public int Property3 { get; set; }
	
	[ProtoMember(4)]
	public string Property4 { get; set; }
	
	[ProtoMember(7)]
	public int Property7 { get; set; }
	
	[ProtoMember(8)]
	public int Property8 { get; set; }
	
	[ProtoMember(9)]
	public int Property9 { get; set; }
	
	[ProtoMember(10)]
	public int[] Property10 { get; set; }
	
	[ProtoMember(11)]
	public int[] Property11 { get; set; }
	
	[ProtoMember(12)]
	public int[] Property12 { get; set; }
	
	[ProtoMember(13)]
	public string Property13 { get; set; }
}

// Define other methods and classes here
public static byte[] StringToByteArray(string hex) {
    return Enumerable.Range(0, hex.Length)
                     .Where(x => x % 2 == 0)
                     .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                     .ToArray();
					 
}

private T Deserialize<T>(byte[] bytes, Type type)
{
  using (var memoryStream = new MemoryStream(bytes))
  {
    return (T)Serializer.NonGeneric.Deserialize(type, memoryStream);
  }
}