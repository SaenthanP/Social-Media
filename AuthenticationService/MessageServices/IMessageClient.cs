using AuthenticationService.Dtos;

namespace AuthenticationService.MessageServices{
    public interface IMessageClient{
        void SendEmail(MessageUserDto messageUserDto);
    }
}