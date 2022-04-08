using EmailService.Models;

namespace EmailService.Events{
    public interface IEventProcessing{
        void SendRegistrationEmail(MessageUserModel messageUserModel);
    }
}