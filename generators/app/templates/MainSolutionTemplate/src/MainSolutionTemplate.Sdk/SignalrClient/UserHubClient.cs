﻿using System;
using System.Collections.Generic;
using System.Reflection;
using MainSolutionTemplate.Shared.Interfaces.Signalr;
using MainSolutionTemplate.Shared.Models;
using Microsoft.AspNet.SignalR.Client;
using log4net;

namespace MainSolutionTemplate.Sdk.SignalrClient
{
	public class UserHubClient : IUserControllerActions
	{
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

	    public UserModel Register(RegisterModel user)
	    {
            return _userHub.Invoke<UserModel>("Register", user).Result;
	    }

	    public bool ForgotPassword(string email)
	    {
            return _userHub.Invoke<bool>("ForgotPassword", email).Result;
	    }

	    #endregion
	}
}