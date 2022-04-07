using System;
using System.Text;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace AuthenticationService.Messaging{
    public class MessageClient : IMessageClient
    {
        private readonly IConfiguration _configuration;
        private readonly IModel _channel;

        public MessageClient(IConfiguration configuration)
        {
            _configuration=configuration;
            
            var factory=new ConnectionFactory(){
                HostName=_configuration.GetSection("RabbitMQ").GetSection("Host").Value,
                Port=int.Parse(_configuration.GetSection("RabbitMQ").GetSection("Port").Value)
            };

            var connection=factory.CreateConnection();
            _channel=connection.CreateModel();
            //TODO move to const
            _channel.ExchangeDeclare(exchange:"event_bus", type:"direct");

            
        }
        public void SendEmail(string content)
        {
            var body=Encoding.UTF8.GetBytes(content);
            _channel.BasicPublish(exchange:"event_bus",routingKey:"email_queue",basicProperties:null,body:body);
            Console.WriteLine("Message sent from Auth Client");
        }
    }
}