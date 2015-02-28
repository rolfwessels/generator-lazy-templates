using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MainSolutionTemplate.Shared.Interfaces.Shared;
using MainSolutionTemplate.Shared.Interfaces.Signalr;
using MainSolutionTemplate.Shared.Models;
using MainSolutionTemplate.Shared.Models.Reference;
using Microsoft.AspNet.SignalR.Client;

namespace MainSolutionTemplate.Sdk.SignalrClient
{
    public class UserHubClient : IUserControllerActions, IUserControllerStandardLookups, ISubscribeUpdateModelClient<UserModel> 
	{
		private readonly IHubProxy _userHub;

		public UserHubClient(HubConnection hubConnection)
		{
			_userHub = hubConnection.CreateHubProxy("UserHub");
		}

		#region Implementation of IUserHub
		
		public Task<UserModel> Get(Guid id)
		{
			return _userHub.Invoke<UserModel>("Get", id);
		}

		public Task<UserModel> Post(UserDetailModel user)
		{
			return _userHub.Invoke<UserModel>("Post", user);
		}

		public Task<UserModel> Put(Guid id, UserDetailModel user)
		{
			return _userHub.Invoke<UserModel>("Put", id, user);
		}

		public Task<bool> Delete(Guid id)
		{
			return _userHub.Invoke<bool>("Delete", id);
		}

	    public Task<UserModel> Register(RegisterModel user)
	    {
            return _userHub.Invoke<UserModel>("Register", user);
	    }

	    public Task<bool> ForgotPassword(string email)
	    {
            return _userHub.Invoke<bool>("ForgotPassword", email);
	    }

	    #endregion

	    #region Implementation of IUserControllerStandardLookups

	    public Task<IList<UserReferenceModel>> Get()
	    {
            return _userHub.Invoke<IList<UserReferenceModel>>("Get");
	    }

	    public Task<IList<UserModel>> GetDetail()
	    {
            return _userHub.Invoke<IList<UserModel>>("GetDetail");
	    }

	    #endregion

        #region Implementation of ISubscribeUpdateModelClient<UserModel>

        public void SubscribeToUpdate(Action<ValueUpdateModel<UserModel>> callBack)
        {
            _userHub.On("OnUpdate", callBack);
            _userHub.Invoke("SubscribeToUpdate");
        }

        public void UnsubscribeFromUpdate()
        {
            _userHub.Invoke("UnsubscribeFromUpdate");
        }

        #endregion
	}
}