using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using FeedService.Models;
using FeedService.EventConstants;
using FeedService.Events;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace FeedService.MessageServices{
    public class MessageClient:BackgroundService{
        private readonly IConfiguration _configuration;
        private readonly IEventProcessing _eventProcessing;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private const string POST_ROUTING_KEY="post";
        private const string NETWORK_ROUTING_KEY="network";
        private const string POST_QUEUE="post_queue";
        private const string NETWORK_QUEUE="network_queue";
        private const string EXCHANGE="event_bus";

        public MessageClient(IConfiguration configuration,IEventProcessing eventProcessing)
        {
            _configuration=configuration;
            _eventProcessing=eventProcessing;

            var factory=new ConnectionFactory(){
                HostName=_configuration.GetSection("RabbitMQHostname").Value,
                Port=int.Parse(_configuration.GetSection("RabbitMQPort").Value)
            };

            _connection=factory.CreateConnection();
            _channel=_connection.CreateModel();

            _channel.ExchangeDeclare(exchange:EXCHANGE,type:"direct");
            _channel.QueueDeclare(POST_QUEUE,true,false,false,null);
            _channel.QueueBind(POST_QUEUE,EXCHANGE,POST_ROUTING_KEY);

            _channel.QueueDeclare(NETWORK_QUEUE,true,false,false,null);
            _channel.QueueBind(NETWORK_QUEUE,EXCHANGE,NETWORK_ROUTING_KEY);
            Console.WriteLine("Consuming the message bus");
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            var consumer=new EventingBasicConsumer(_channel);
            consumer.Received+=(model,ea)=>{
                var body=ea.Body.ToArray();
                var jsonMessage=Encoding.UTF8.GetString(body);

                if(ea.RoutingKey==POST_ROUTING_KEY){
                    processPostEvent(JsonConvert.DeserializeObject<PublishedPostModel>(jsonMessage));
                }else if(ea.RoutingKey==NETWORK_ROUTING_KEY){
                    processNetworkEvent(JsonConvert.DeserializeObject<PublishedNetworkModel>(jsonMessage));
                }
              
                Console.WriteLine("hit the consumer");
            };

            _channel.BasicConsume(queue:POST_QUEUE,
                autoAck:true,
                consumer:consumer,
                exclusive: true
            );

            _channel.BasicConsume(queue:NETWORK_QUEUE,
                autoAck:true,
                consumer:consumer,
                exclusive: true
            );
            return Task.CompletedTask;
        }
        private void processPostEvent(PublishedPostModel publishedPostModel){

            switch(publishedPostModel.EventType){
                case PostConstants.CREATE_POST:
                    _eventProcessing.AddPostToCache(publishedPostModel);
                    break;
            }
            
        }
        private void processNetworkEvent(PublishedNetworkModel publishedNetworkModel){
            switch(publishedNetworkModel.EventType){
                case NetworkConstants.FOLLOW_USER:
                    _eventProcessing.AddFollowToCache(publishedNetworkModel);
                    break;
                case NetworkConstants.UNFOLLOW_USER:
                    _eventProcessing.RemoveFollowFromCache(publishedNetworkModel);
                    break;
            }
           
        }
    }
}