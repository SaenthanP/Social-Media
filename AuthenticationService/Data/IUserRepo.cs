
using AuthenticationService.Dtos;

namespace AuthenticationService.Data{
    public interface IUserRepo{
        bool SaveChanges();
        void CreateUser(CreateUserDto createUserDto);
        ReadUserDto GetUserByEmail(string email);
        bool IsUsernameExists(string username);
        bool IsEmailExists(string email);
        bool IsEmailValid(string email);
    
    }
}