using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using MainSolutionTemplate.Shared.Interfaces.Signalr;
using MainSolutionTemplate.Shared.Models;
using MainSolutionTemplate.Shared.Models.Reference;
using Microsoft.AspNet.SignalR.Client;
using log4net;

namespace MainSolutionTemplate.Sdk.SignalrClient
{
	public class UserHubClient : IUserControllerActions , IUserControllerStandardLookups
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

	    public Task<List<UserReferenceModel>> Get()
	    {
            return _userHub.Invoke<List<UserReferenceModel>>("Get");
	    }

	    public Task<List<UserModel>> GetDetail()
	    {
            return _userHub.Invoke<List<UserModel>>("GetDetail");
	    }

	    #endregion
	}
}