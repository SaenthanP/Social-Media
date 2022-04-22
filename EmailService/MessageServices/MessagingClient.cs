using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using EmailService.EventConstants;
using EmailService.Models;
using System.Text.Json;
using System.Net.Mail;
using System.Net;
using EmailService.Events;

namespace EmailService.MessageServices{
   public class MessagingClient:BackgroundService{
        private readonly IConfiguration _configuration;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly IEventProcessing _eventProcessing;
        private const string EXCHANGE="event_bus";
        private const string ROUTING_KEY="email";
        private const string EMAIL_QUEUE="email_queue";
        public MessagingClient(IConfiguration configuration, IEventProcessing eventProcessing)
       {
           _configuration=configuration;
            var factory=new ConnectionFactory(){
                HostName=_configuration.GetSection("RabbitMQHostname").Value,
                Port=int.Parse(_configuration.GetSection("RabbitMQPort").Value)
            };

            _connection=factory.CreateConnection();
            _channel=_connection.CreateModel();

            _channel.ExchangeDeclare(exchange:EXCHANGE, type:"direct");
           
            _channel.QueueDeclare(EMAIL_QUEUE,true,false,false,null);
            _channel.QueueBind(EMAIL_QUEUE, EXCHANGE, ROUTING_KEY);
             Console.WriteLine("Consuming message bus");
             _eventProcessing=eventProcessing;
       }
       protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {                 
             stoppingToken.ThrowIfCancellationRequested();
             var consumer=new EventingBasicConsumer(_channel);
            //RHS is subscribed to the  LHS. When LHS is called, RHS is called.
             consumer.Received+=(model,ea)=>{
                 var body=ea.Body.ToArray();
                 var jsonMessage=Encoding.UTF8.GetString(body);
                 var messageUserModel=JsonSerializer.Deserialize<MessageUserModel>(jsonMessage);
                 Console.WriteLine("message: "+ jsonMessage);
                 processEvent(messageUserModel);
                
             };

             _channel.BasicConsume(queue:EMAIL_QUEUE,
                    autoAck:true,
                    consumer:consumer,
                    exclusive:true
                    );

            return Task.CompletedTask;
        }

        private void processEvent(MessageUserModel messageUserModel){
            Console.WriteLine(_configuration["test"]);
            if(messageUserModel.EventType==EmailConstants.EMAIL_ON_REGISRATION){
                Console.WriteLine("msg processed");
                _eventProcessing.SendRegistrationEmail(messageUserModel);
            }
        }
  
   } 
}
