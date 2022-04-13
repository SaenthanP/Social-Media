using System;
using System.Net;
using System.Net.Mail;
using PersonalNetworkService.Models;
using Microsoft.Extensions.Configuration;
using PersonalNetworkService.Services;

namespace PersonalNetworkService.Events{
    public class EventProcessing : IEventProcessing
    {
        private readonly IConfiguration _configuration;
        private readonly IUserNetworkService _userNetworkService;

        public EventProcessing(IConfiguration configuration, IUserNetworkService userNetworkService)
        {
            _configuration=configuration;
            _userNetworkService=userNetworkService;
        }
        public void AddUserToNetwork(MessageUserModel messageUserModel)
        {
          Console.WriteLine("Added user to netwokr");
          _userNetworkService.AddUserToNetwork(messageUserModel);
          
        }
    }
}