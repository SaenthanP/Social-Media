using PersonalNetworkService.Models;

namespace PersonalNetworkService.Services{
    public interface IUserNetworkService{
        void AddUserToNetwork(MessageUserModel messageUserModel);
    }
}