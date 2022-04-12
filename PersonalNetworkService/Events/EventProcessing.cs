using System;
using System.Net;
using System.Net.Mail;
using PersonalNetworkService.Models;
using Microsoft.Extensions.Configuration;

namespace PersonalNetworkService.Events{
    public class EventProcessing : IEventProcessing
    {
        private readonly IConfiguration _configuration;

        public EventProcessing(IConfiguration configuration)
        {
            _configuration=configuration;
        }
        public void AddUserToNetwork(MessageUserModel messageUserModel)
        {
          Console.WriteLine("Added user to netwokr");
        }
    }
}