using System.Threading.Tasks;
using MainSolutionTemplate.Dal.Models;

namespace MainSolutionTemplate.Core.BusinessLogic.Components.Interfaces
{
    public interface IUserManager : IBaseManager<User>
    {
        Task<User> Save(User user, string password);
        Task<User> GetUserByEmailAndPassword(string email, string password);
        Task<User> GetUserByEmail(string email);
        Task UpdateLastLoginDate(string email);
    }
}