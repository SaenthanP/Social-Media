using System;
using EmailService.Models;

namespace EmailService.Events{
    public class EventProcessing : IEventProcessing
    {
        public void SendRegistrationEmail(MessageUserModel messageUserModel)
        {
            Console.WriteLine("Sending email to user");
        }
    }
}