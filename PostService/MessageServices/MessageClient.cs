using System;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using PostService.Dtos;
using RabbitMQ.Client;

namespace PostService.MessageServices{
    public class MessageClient : IMessageClient
    {
        private readonly IConfiguration _configuration;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private const string EXCHANGE="event_bus";

        public MessageClient(IConfiguration Configuration)
        {
            _configuration=Configuration;
            var factory=new ConnectionFactory(){
                HostName=_configuration.GetSection("RabbitMQHostname").Value,
                Port=int.Parse(_configuration.GetSection("RabbitMQPort").Value)
            };
            
            _connection=factory.CreateConnection();
            _channel=_connection.CreateModel();
            _channel.ExchangeDeclare(exchange:EXCHANGE,"direct");

        }
        public void PostCreated(PublishPostDto publishPostDto)
        {
            var body=Encoding.UTF8.GetBytes(JsonSerializer.Serialize(publishPostDto));
            _channel.BasicPublish(EXCHANGE,routingKey:"post",basicProperties:null,body:body);
            Console.WriteLine("Post created");
        }
    }
}