using MainSolutionTemplate.Dal.Models;

namespace MainSolutionTemplate.Core.BusinessLogic.Components.Interfaces
{
    public interface IUserManager : IBaseManager<User>
    {
        User Save(User user, string password);
        User GetUserByEmailAndPassword(string email, string password);
        User GetUserByEmail(string email);
        void UpdateLastLoginDate(string email);
    }
}