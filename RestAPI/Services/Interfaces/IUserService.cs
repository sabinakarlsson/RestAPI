using RestAPI.Data;
using RestAPI.ViewModel;
using System.Threading.Tasks;

namespace RestAPI.Services.Interfaces
{
    public interface IUserService
    {
        Task<User> GetUserAsync(string userName);
        Task AddUserAsync(User user);

    }
}
