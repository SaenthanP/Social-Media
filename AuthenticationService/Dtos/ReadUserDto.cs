using System.ComponentModel.DataAnnotations;

namespace AuthenticationService.Dtos{
    public class ReadUserDto{
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
 
    }
}