using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using PersonalNetworkService.EventConstants;
using PersonalNetworkService.Models;
using System.Text.Json;
using System.Net.Mail;
using System.Net;
using PersonalNetworkService.Events;

namespace PersonalNetworkService.MessageServiceConsumer{
   public class MessagingClient:BackgroundService{
        private readonly IConfiguration _configuration;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly IEventProcessing _eventProcessing;
        private const string exchange="event_bus";
        private const string queue="add_to_network_queue";
        public MessagingClient(IConfiguration configuration, IEventProcessing eventProcessing)
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
            _channel.QueueBind(queue, exchange, "create_user");
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

             _channel.BasicConsume(queue:queue,
                    autoAck:true,
                    consumer:consumer
                    );

            return Task.CompletedTask;
        }

        private void processEvent(MessageUserModel messageUserModel){
            if(messageUserModel.EventType==PersonalNetworkConstants.CREATE_USER){
                Console.WriteLine("msg processed");
                _eventProcessing.AddUserToNetwork(messageUserModel);
            }
        }
  
   } 
}
