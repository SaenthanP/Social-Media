
using AuthenticationService.Dtos;
using AuthenticationService.Models;

namespace AuthenticationService.Data{
    public interface IUserRepo{
        bool SaveChanges();
        void CreateUser(CreateUserDto createUserDto);
        ReadUserDto GetUserByEmail(string email);
        ReadUserDto GetUserByUsername(string username);
        bool IsUsernameExists(string username);
        bool IsEmailExists(string email);
        bool IsEmailValid(string email);
        bool IsUserMatch(LoginUserDto UserDto);
    
    }
}