using AuthenticationService.Dtos;

namespace AuthenticationService.MessageServices{
    public interface IMessageClient{
        void SendEmail(MessageUserDto messageUserDto);
        void CreateUserInNetwork(MessageUserDto messageUserDto);
    }
}