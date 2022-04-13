
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var factory = new ConnectionFactory();
factory.Uri = new Uri(""); //write AMQP URL

using (var connection = factory.CreateConnection())
{
    var channel = connection.CreateModel();

    //channel.ExchangeDeclare("exchange-fanout-logs", durable: true, type: ExchangeType.Fanout);

    var rndQueuename = channel.QueueDeclare().QueueName;

    channel.QueueBind(rndQueuename, "exchange-fanout-logs", "", null);

    channel.BasicQos(0,1,false);

    var subscriber = new EventingBasicConsumer(channel);

    channel.BasicConsume(rndQueuename, false, subscriber);

    Console.WriteLine("Listening...");

    subscriber.Received += (object? sender, BasicDeliverEventArgs e) =>
    {
        var message = Encoding.UTF8.GetString(e.Body.ToArray());

        Thread.Sleep(500);

        Console.WriteLine($"Received Message : {message}");

        channel.BasicAck(e.DeliveryTag, false);
    };

    Console.ReadLine();
}