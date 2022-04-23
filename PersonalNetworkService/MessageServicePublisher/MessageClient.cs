using System;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using PersonalNetworkService.EventConstants;
using PersonalNetworkService.Models;
using RabbitMQ.Client;

namespace PersonalNetworkService.MessageServicePublisher{
    public class MessageClient : IMessageClient
    {
        private readonly IConfiguration _configuration;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        private const string EXCHANGE="event_bus";
        private const string ROUTING_KEY="network";
 
        public MessageClient(IConfiguration configuration)
        {
            _configuration=configuration;
            
            var factory=new ConnectionFactory(){
                HostName=_configuration.GetSection("RabbitMQHostname").Value,
                Port=int.Parse(_configuration.GetSection("RabbitMQPort").Value)
            };

            _connection=factory.CreateConnection();
            _channel=_connection.CreateModel();
            _channel.ExchangeDeclare(exchange:EXCHANGE,"direct");
        }
        public void FollowUser(PublishNetworkModel publishNetworkModel)
        {
            publishNetworkModel.EventType=PersonalNetworkConstants.FOLLOW_USER;
            PublishEvent(publishNetworkModel);
            Console.WriteLine("Follow user event has been published");
        }

        public void UnfollowUser(PublishNetworkModel publishNetworkModel)
        {
            publishNetworkModel.EventType=PersonalNetworkConstants.UNFOLLOW_USER;
            PublishEvent(publishNetworkModel);
            Console.WriteLine("Unfollow user event has been published");
        }
        private void PublishEvent(PublishNetworkModel publishNetworkModel){
            var body=Encoding.UTF8.GetBytes(JsonSerializer.Serialize(publishNetworkModel));
            _channel.BasicPublish(EXCHANGE,ROUTING_KEY,basicProperties:null,body:body);
        }
    }
}