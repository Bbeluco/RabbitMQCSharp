using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory { HostName = "localhost" };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();
string queueName = channel.QueueDeclare().QueueName;

channel.QueueBind(queue: queueName, 
                 exchange: "logs",
                 routingKey: string.Empty);

System.Console.WriteLine("Waiting for messages...");

var consumer = new EventingBasicConsumer(channel);
consumer.Received += (model, ea) => {
    var body = ea.Body.ToArray();
    string message = Encoding.UTF8.GetString(body);

    System.Console.WriteLine($"Message received: {message}");
};

channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
System.Console.WriteLine("Press [enter] to exit");
Console.ReadLine();