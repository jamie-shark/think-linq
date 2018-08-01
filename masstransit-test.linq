<Query Kind="Program">
  <Reference>C:\Code\innovation\cqrs-starter-kit\sample-app\Edument.CQRS\bin\Debug\Edument.CQRS.dll</Reference>
  <Reference>C:\Code\innovation\cqrs-starter-kit\sample-app\Edument.CQRS\bin\Debug\GreenPipes.dll</Reference>
  <Reference>C:\Code\innovation\cqrs-starter-kit\sample-app\Edument.CQRS\bin\Debug\MassTransit.dll</Reference>
  <Reference>C:\Code\innovation\cqrs-starter-kit\sample-app\Edument.CQRS\bin\Debug\MassTransit.RabbitMqTransport.dll</Reference>
  <Reference>C:\Code\innovation\cqrs-starter-kit\sample-app\Edument.CQRS\bin\Debug\Microsoft.Diagnostics.Tracing.EventSource.dll</Reference>
  <Reference>C:\Code\innovation\cqrs-starter-kit\sample-app\Edument.CQRS\bin\Debug\NewId.dll</Reference>
  <Reference>C:\Code\innovation\cqrs-starter-kit\sample-app\Edument.CQRS\bin\Debug\Newtonsoft.Json.Bson.dll</Reference>
  <Reference>C:\Code\innovation\cqrs-starter-kit\sample-app\Edument.CQRS\bin\Debug\Newtonsoft.Json.dll</Reference>
  <Reference>C:\Code\innovation\cqrs-starter-kit\sample-app\Edument.CQRS\bin\Debug\nunit.framework.dll</Reference>
  <Reference>C:\Code\innovation\cqrs-starter-kit\sample-app\Edument.CQRS\bin\Debug\RabbitMQ.Client.dll</Reference>
  <Reference>C:\Code\innovation\cqrs-starter-kit\sample-app\Edument.CQRS\bin\Debug\StructureMap.dll</Reference>
  <Reference>C:\Code\innovation\cqrs-starter-kit\sample-app\Edument.CQRS\bin\Debug\StructureMap.Net4.dll</Reference>
  <Reference>C:\Windows\Microsoft.NET\Framework64\v4.0.30319\System.Linq.Parallel.dll</Reference>
  <Reference>C:\Windows\Microsoft.NET\Framework64\v4.0.30319\System.Net.dll</Reference>
  <Reference>C:\Windows\Microsoft.NET\Framework64\v4.0.30319\System.Net.Http.dll</Reference>
  <Reference>C:\Windows\Microsoft.NET\Framework64\v4.0.30319\System.Net.Http.WebRequest.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Threading.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Threading.Tasks.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Threading.Thread.dll</Reference>
  <Reference>C:\Code\innovation\cqrs-starter-kit\sample-app\Edument.CQRS\bin\Debug\System.ValueTuple.dll</Reference>
  <Namespace>MassTransit</Namespace>
  <Namespace>System</Namespace>
  <Namespace>System.Threading</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

public static void Main()
{
	var messages = new object[]
	{
		new Test.YourMessage{Text = "Hi"},
		new Test.YourMessage{Text = "Ooer"},
		new Test.SomeEvent{Id = "1"},
		new Test.SomeEvent{Id = "2"},
		new Test.SomeEvent{Id = "3"},
		new Test.SomeEvent{Id = "4"},
		new Test.SomeEvent{Id = "5"}
	};
	
	Task.Run(async () =>
	{
		await new BusTest().Test(async bus =>
		{
			foreach (var message in messages)
			{
				await bus.Publish(message);
			}
		});
	});
}

public class BusTest
{
	public async Task Test(Func<IBusControl, Task> publishEvents)
	{
	    var bus = Bus.Factory.CreateUsingRabbitMq(sbc =>
	    {
	        var host = sbc.Host(new Uri("rabbitmq://localhost:5673/"), h =>
	        {
	            h.Username("guest");
	            h.Password("guest");
	        });
	
	        sbc.ReceiveEndpoint(host, "Test", ep =>
	        {
	            ep.Handler<Test.YourMessage>(context =>
	            {
	                return Console.Out.WriteLineAsync($"Received: {context.Message.Text}");
	            });
				
	            ep.Handler<Test.SomeEvent>(context =>
	            {
	                return Console.Out.WriteLineAsync($"Event: {context.Message.Id}");
	            });
	        });
	    });
	
	    await bus.StartAsync();
		await publishEvents(bus);
	    await bus.StopAsync();
	}
}

}

namespace Test
{
	public class YourMessage
	{
		public string Text { get; set; }
	}
	
	public class SomeEvent
	{
		public string Id { get; set; }
	}
}

class EOF {