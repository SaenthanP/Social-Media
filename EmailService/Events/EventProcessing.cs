using System;
using System.Net;
using System.Net.Mail;
using EmailService.Models;
using Microsoft.Extensions.Configuration;

namespace EmailService.Events{
    public class EventProcessing : IEventProcessing
    {
        private readonly IConfiguration _configuration;

        public EventProcessing(IConfiguration configuration)
        {
            _configuration=configuration;
        }
        public void SendRegistrationEmail(MessageUserModel messageUserModel)
        {
            var client = new SmtpClient(_configuration["MailTrapHost"], int.Parse(_configuration["MailTrapPort"]))
            {
                Credentials = new NetworkCredential(_configuration["MailTrapUsername"], _configuration["MailTrapPassword"]),
                EnableSsl = true
            };
            client.Send("support@social.com", messageUserModel.Email, "Thank you for registering your account!", $"Hello {messageUserModel.Username} thank you for joining our platform. We hope you enjoy your time! Please reach out if you have any questions!");
            Console.WriteLine("Email Sent");
        }
    }
}