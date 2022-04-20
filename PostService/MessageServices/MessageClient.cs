using System;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace PostService.MessageServices{
    public class MessageServices : IMessageClient
    {
        private readonly IConfiguration _configuration;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private const string exchange="event_bus";

        public MessageServices(IConfiguration Configuration)
        {
            _configuration=Configuration;
            var factory=new ConnectionFactory(){
                HostName=_configuration.GetSection("HostName").Value,
                Port=int.Parse(_configuration.GetSection("Port").Value)
            };
            
            _connection=factory.CreateConnection();
            _channel=_connection.CreateModel();
            _channel.ExchangeDeclare(exchange:exchange,"direct");

        }
        public void PostCreated()
        {
            var test="testing1";
            var body=Encoding.UTF8.GetBytes(JsonSerializer.Serialize(test));
            _channel.BasicPublish(exchange,routingKey:"post",basicProperties:null,body:body);
            Console.WriteLine("Post created");
        }
    }
}