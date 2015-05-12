using System.Threading.Tasks;
using MainSolutionTemplate.Sdk.SignalrClient.Base;
using MainSolutionTemplate.Shared.Interfaces.Shared;
using MainSolutionTemplate.Shared.Models;
using MainSolutionTemplate.Shared.Models.Reference;
using Microsoft.AspNet.SignalR.Client;

namespace MainSolutionTemplate.Sdk.SignalrClient
{
    public class UserHubClient : BaseCrudHubClient<UserModel, UserReferenceModel, UserDetailModel>,
                                 IUserControllerActions, IUserControllerStandardLookups
    {
        public UserHubClient(HubConnection hubConnection) : base(hubConnection)
        {
        }

        protected override string HubName()
        {
            return "UserHub";
        }

        #region Implementation of IUserHub

        public Task<UserModel> Register(RegisterModel user)
        {
            return _userHub.Invoke<UserModel>("Register", user);
        }

        public Task<bool> ForgotPassword(string email)
        {
            return _userHub.Invoke<bool>("ForgotPassword", email);
        }

        #endregion
    }
}