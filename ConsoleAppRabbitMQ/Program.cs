
using RabbitMQ.Client;
using System.Text;

var factory = new ConnectionFactory();
factory.Uri = new Uri(""); //write AMQP URL

string messageRead;

do
{
    Console.Write("Please write your message for 50 times: ");
    messageRead = Console.ReadLine();
    if (messageRead != string.Empty && messageRead != null)
    {
        PublishMessage(messageRead);
    }
    else
    {
        Environment.Exit(0);
    }

} while (messageRead != null);

void PublishMessage(string message)
{
    try
    {
        using (var connection = factory.CreateConnection())
        {
            var channel = connection.CreateModel();

            channel.ExchangeDeclare("exchange-fanout-logs",durable:true,type:ExchangeType.Fanout);

            Enumerable.Range(0, 40).ToList().ForEach(x =>
            {
                var messageBody = Encoding.UTF8.GetBytes(message+$"_{x}");

                channel.BasicPublish("exchange-fanout-logs","", null, messageBody);

                Console.WriteLine($"'{message}-{x}' is sended");
            });

            Console.WriteLine("All messages are sended");
        }
    }
    catch (Exception ex) { Console.WriteLine(ex.ToString()); }


}


