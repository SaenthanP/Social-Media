using System.ComponentModel.DataAnnotations;

namespace AuthenticationService.Dtos{
    public class LoginUserDto{
   
        public string Username { get; set; }
        public string Password { get; set; }
 
    }
}