using PersonalNetworkService.Models;

namespace PersonalNetworkService.Events{
    public interface IEventProcessing{
        void AddUserToNetwork(MessageUserModel messageUserModel);
    }
}