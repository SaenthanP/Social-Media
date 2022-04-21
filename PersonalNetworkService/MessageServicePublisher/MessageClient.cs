using System;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using PersonalNetworkService.Dtos;
using PersonalNetworkService.Models;
using RabbitMQ.Client;

namespace PersonalNetworkService.MessageServicePublisher{
    public class MessageClient : IMessageClient
    {
        private readonly IConfiguration _configuration;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        private const string exchange="event_bus";
 
        public MessageClient(IConfiguration configuration)
        {
            _configuration=configuration;
            
            var factory=new ConnectionFactory(){
                HostName=_configuration.GetSection("RabbitMQHostname").Value,
                Port=int.Parse(_configuration.GetSection("RabbitMQPort").Value)
            };

            _connection=factory.CreateConnection();
            _channel=_connection.CreateModel();
            _channel.ExchangeDeclare(exchange:exchange,"direct");
        }
        public void FollowUser(PublishFeedModel publishFeedModel)
        {
            var body=Encoding.UTF8.GetBytes(JsonSerializer.Serialize(publishFeedModel));
            _channel.BasicPublish(exchange,"network");
            Console.WriteLine("Follow user event has been published");
        }
    }
}