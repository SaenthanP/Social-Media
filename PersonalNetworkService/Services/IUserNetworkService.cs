using System.Threading.Tasks;
using PersonalNetworkService.Models;

namespace PersonalNetworkService.Services{
    public interface IUserNetworkService{
        Task AddUserToNetwork(MessageUserModel messageUserModel);
        Task FollowUser(string userToFollowId,string userId);
        Task<bool> IsFollowingUser(string userToCheck,string userId);
    }
}