using System.ComponentModel.DataAnnotations;

namespace AuthenticationService.Dtos{
    public class ReadUserDto{
        public string Key { get; set; }
        public string Username { get; set; }
 
    }
}