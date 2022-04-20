using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace FeedService.MessageServices{
    public class MessageClient:BackgroundService{
        private readonly IConfiguration _configuration;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private const string routingKey="post";
        private const string queue="post_queue";
        private const string exchange="event_bus";

        public MessageClient(IConfiguration configuration)
        {
            _configuration=configuration;

            var factory=new ConnectionFactory(){
                HostName=_configuration.GetSection("RabbitMQHostname").Value,
                Port=int.Parse(_configuration.GetSection("Port").Value)
            };

            _connection=factory.CreateConnection();
            _channel=_connection.CreateModel();

            _channel.ExchangeDeclare(exchange:exchange,type:"direct");
            _channel.QueueDeclare(queue,true,false,false,null);
            _channel.QueueBind(queue,exchange,routingKey);
            Console.WriteLine("Consuming the message bus");
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            var consumer=new EventingBasicConsumer(_channel);
            consumer.Received+=(model,ea)=>{
                var body=ea.Body.ToArray();
                var jsonMesage=Encoding.UTF8.GetString(body);
            };

            _channel.BasicConsume(queue:queue,
                autoAck:true,
                consumer:consumer,
                exclusive: true
            );
            return Task.CompletedTask;
        }
    }
}