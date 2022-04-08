using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace EmailService.MessageServices{
   public class MessagingClient:BackgroundService{
        private readonly IConfiguration _configuration;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private const string exchange="event_bus";
        private const string routingKey="email";
        private const string queue="email_queue";
        public MessagingClient(IConfiguration configuration)
       {
           _configuration=configuration;
            var factory=new ConnectionFactory(){
                HostName=_configuration.GetSection("RabbitMQHostname").Value,
                Port=int.Parse(_configuration.GetSection("RabbitMQPort").Value)
            };

            _connection=factory.CreateConnection();
            _channel=_connection.CreateModel();

            _channel.ExchangeDeclare(exchange:exchange, type:"direct");
            _channel.QueueDeclare(queue,true,false,false,null);
            _channel.QueueBind(queue, exchange, routingKey);

             Console.WriteLine("Consuming message bus");
       }
       protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {                 
             stoppingToken.ThrowIfCancellationRequested();
             var consumer=new EventingBasicConsumer(_channel);
            //RHS is subscribed to the  LHS. When LHS is called, RHS is called.
             consumer.Received+=(model,ea)=>{
                 var body=ea.Body.ToArray();
                 var message=Encoding.UTF8.GetString(body);
                 Console.WriteLine("message: "+ message);
             };

             _channel.BasicConsume(queue:queue,
                    autoAck:true,
                    consumer:consumer
                    );

            return Task.CompletedTask;
        }
  
   } 
}