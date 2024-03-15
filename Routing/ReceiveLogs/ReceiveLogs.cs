using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory { HostName = "localhost" };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.ExchangeDeclare(exchange: "direct_logs", type: ExchangeType.Direct);

if(args.Length < 1) {
    Console.WriteLine($"usage: dotnet run [info] [warning] [error]");
    System.Console.WriteLine("Press [enter] to exit");
    Console.ReadLine();
    Environment.ExitCode = 1;

    return;
}

string queueName = channel.QueueDeclare().QueueName;
foreach(var severity in args) {
    channel.QueueBind(queue: queueName
                     ,exchange: "direct_logs",
                     routingKey: severity);
}

var consumer = new EventingBasicConsumer(channel);
consumer.Received += (model, ea) => {
    byte[] body = ea.Body.ToArray();
    string message = Encoding.UTF8.GetString(body);
    var routingKey = ea.RoutingKey;

    Console.WriteLine($"Severity: ${routingKey} / Message: {message}");
};

channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
Console.WriteLine("Press [enter] to exit");
Console.ReadLine();