using System;
using System.Text;
using System.Text.Json;
using AuthenticationService.Dtos;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace AuthenticationService.MessageServices{
    public class MessageClient : IMessageClient
    {
        private readonly IConfiguration _configuration;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private const string EXCHANGE="event_bus";
        public MessageClient(IConfiguration configuration)
        {
            _configuration=configuration;
            
            var factory=new ConnectionFactory(){
                HostName=_configuration.GetSection("RabbitMQHostname").Value,
                Port=int.Parse(_configuration.GetSection("RabbitMQPort").Value)
            };
        
            _connection=factory.CreateConnection();
            _channel=_connection.CreateModel();
            _channel.ExchangeDeclare(exchange:EXCHANGE, type:"direct");

            Console.WriteLine("Message bus connection established");
            
        }
        public void SendEmail(MessageUserDto messageUserDto)
        {
            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(messageUserDto));
            
            _channel.BasicPublish(exchange:EXCHANGE,routingKey:"email",basicProperties:null,body:body);
            Console.WriteLine("Email message published from authentication service");
        }

        public void CreateUserInNetwork(MessageUserDto messageUserDto)
        {
            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(messageUserDto));
            
            _channel.BasicPublish(exchange:EXCHANGE,routingKey:"create_user",basicProperties:null,body:body);
            Console.WriteLine("Create User has been published from authentication service");
        }
    }
}