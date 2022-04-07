using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace EmailService.MessagingClient{
   public class MessagingClient:BackgroundService{
        private readonly IConfiguration _configuration;
        private readonly IModel _channel;

        public MessagingClient(IConfiguration configuration)
       {
           _configuration=configuration;
            var factory=new ConnectionFactory(){
                HostName=_configuration.GetSection("RabbitMQHostName").Value,
                Port=int.Parse(_configuration.GetSection("RabbitMQPort").Value)
            };

            var connection=factory.CreateConnection();
            _channel=connection.CreateModel();
            //TODO move to const
            _channel.ExchangeDeclare(exchange:"event_bus", type:"direct");

            var queueName=_channel.QueueDeclare().QueueName;
            _channel.QueueBind(queue:queueName,
                            exchange:"event_bus",
                            routingKey:"email_queue");
            
       }
       protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
             stoppingToken.ThrowIfCancellationRequested();
             var consumer=new EventingBasicConsumer(_channel);
             consumer.Received+=(model,ea)=>{
                 var body=ea.Body.ToArray();
                 var message=Encoding.UTF8.GetString(body);
                 Console.WriteLine("message: "+ message);
             };
             _channel.BasicConsume(queue:_channel.QueueDeclare().QueueName,
                    autoAck:true,
                    consumer:consumer
                    );
            return Task.CompletedTask;
        }
   } 
}