using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory{ HostName = "localhost" };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.QueueDeclare(queue: "hello",
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

System.Console.WriteLine("[*] Waiting for message");

var consumer = new EventingBasicConsumer(channel);
consumer.Received += (model, ea) => {
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    System.Console.WriteLine($"Message Received: {message}");
};

channel.BasicConsume(queue: "hello",
                    autoAck: true,
                    consumer: consumer);

System.Console.WriteLine("Press [enter] to exit");
Console.ReadLine();