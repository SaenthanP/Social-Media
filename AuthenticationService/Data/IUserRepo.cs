
using AuthenticationService.Dtos;

namespace AuthenticationService.Data{
    public interface IUserRepo{
        bool SaveChanges();
        ReadUserDto CreateUser(CreateUserDto createUserDto);
        bool IsUsernameExists(string username);

        bool IsEmailExists(string email);
    
    }
}