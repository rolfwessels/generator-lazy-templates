using System;
using System.Collections.Generic;
using System.Reflection;
using MainSolutionTemplate.Api.SignalR;
using MainSolutionTemplate.Shared.Models;
using Microsoft.AspNet.SignalR.Client;
using log4net;

namespace MainSolutionTemplate.Api.Tests.SignalrClient
{
	public class UserHubClient : IUserHub
	{
		private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private readonly IHubProxy _userHub;

		public UserHubClient(HubConnection hubConnection)
		{
			_userHub = hubConnection.CreateHubProxy("UserHub");
		}

		#region Implementation of IUserHub
		
		public List<UserModel> Get()
		{
			return _userHub.Invoke<List<UserModel>>("Get").Result;
		}

		public UserModel Get(Guid id)
		{
			return _userHub.Invoke<UserModel>("Get", id).Result;
		}

		public UserModel Post(UserDetailModel user)
		{
			var invoke = _userHub.Invoke<UserModel>("Post", user);
			return invoke.Result;
		}

		public UserModel Put(Guid id, UserDetailModel user)
		{
			return _userHub.Invoke<UserModel>("Put", id, user).Result;
		}

		public bool Delete(Guid id)
		{
			return _userHub.Invoke<bool>("Delete", id).Result;
		}

		#endregion
	}
}