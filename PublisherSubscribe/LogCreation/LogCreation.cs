using System.Text;
using RabbitMQ.Client;

var factory = new ConnectionFactory{ HostName = "localhost" };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.ExchangeDeclare(exchange: "logs", type: ExchangeType.Fanout);
string queueName = channel.QueueDeclare().QueueName;

string message = "[*] LOG INFORMATION [*]";
byte[] body = Encoding.UTF8.GetBytes(message);

channel.BasicPublish(exchange: "logs",
                    routingKey: string.Empty,
                    basicProperties: null,
                    body: body);

System.Console.WriteLine("Success send! Press [enter] to exit");
Console.ReadLine();