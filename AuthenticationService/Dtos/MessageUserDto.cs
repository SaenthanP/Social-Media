using System.ComponentModel.DataAnnotations;

namespace AuthenticationService.Dtos{
    public class MessageUserDto{
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string EventType{get; set;}
 
    }
}