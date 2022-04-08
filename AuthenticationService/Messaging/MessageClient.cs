using System;
using System.Text;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace AuthenticationService.Messaging{
    public class MessageClient : IMessageClient
    {
        private readonly IConfiguration _configuration;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private const string exchange="event_bus";
        private const string routingKey="email";
        public MessageClient(IConfiguration configuration)
        {
            _configuration=configuration;
            
            var factory=new ConnectionFactory(){
                HostName=_configuration.GetSection("RabbitMQHostname").Value,
                Port=int.Parse(_configuration.GetSection("RabbitMQPort").Value)
            };
        
            _connection=factory.CreateConnection();
            _channel=_connection.CreateModel();
            _channel.ExchangeDeclare(exchange:exchange, type:"direct");

            Console.WriteLine("Message bus connection established");
            
        }
        public void SendEmail(string content)
        {
            var body=Encoding.UTF8.GetBytes(content);
            _channel.BasicPublish(exchange:exchange,routingKey:routingKey,basicProperties:null,body:body);
            Console.WriteLine("Email message published from authentication service");
        }

    }
}