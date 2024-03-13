using System.Text;
using RabbitMQ.Client;

class NewTasks {
    static void Main(string[] args){
        var factory = new ConnectionFactory{ HostName = "localhost" };
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare(queue: "tasks_queue",
                            durable: true,
                            exclusive: false,
                            autoDelete: false,
                            arguments: null);

        string message = GetMessage(args);
        var body = Encoding.UTF8.GetBytes(message);

        var properties = channel.CreateBasicProperties();
        properties.Persistent = true;

        channel.BasicPublish(exchange: string.Empty,
                            routingKey: "tasks_queue",
                            basicProperties: properties,
                            body: body);
                        
        System.Console.WriteLine($"message sent: {message}");

        System.Console.WriteLine("Press [enter] to exit");
        Console.ReadLine();

    }
    private static string GetMessage(string[] args) {
        return (args.Length > 0) ? string.Join(",", args) : "Hello World!";
    }
}