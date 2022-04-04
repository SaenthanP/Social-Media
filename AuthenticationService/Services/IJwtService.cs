using AuthenticationService.Dtos;

namespace AuthenticationService.Services{
    public interface IJwtService{
        string IssueToken(ReadUserDto readUserDto);
    }
}