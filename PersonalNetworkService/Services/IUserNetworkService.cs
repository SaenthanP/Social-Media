using System.Threading.Tasks;
using PersonalNetworkService.Models;

namespace PersonalNetworkService.Services{
    public interface IUserNetworkService{
        Task AddUserToNetwork(MessageUserModel messageUserModel);
    }
}