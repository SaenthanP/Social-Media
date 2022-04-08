using System.ComponentModel.DataAnnotations;

namespace EmailService.Models{
    public class MessageUserModel{
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string EventType{get; set;}
 
    }
}