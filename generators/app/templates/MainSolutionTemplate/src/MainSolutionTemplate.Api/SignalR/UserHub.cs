using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MainSolutionTemplate.Api.AppStartup;
using MainSolutionTemplate.Api.Models;
using MainSolutionTemplate.Api.SignalR.Attributes;
using MainSolutionTemplate.Api.WebApi.Attributes;
using MainSolutionTemplate.Api.WebApi.Controllers;
using MainSolutionTemplate.Dal.Models.Enums;
using log4net;

namespace MainSolutionTemplate.Api.SignalR
{
	[TokenAuthorize]
	public class UserHub : BaseHub , IUserHub
	{
		private readonly UserController _userController;

		public UserHub()
		{
			_userController = IocContainerSetup.Instance.Resolve<UserController>();
		}

		[HubAuthorizeActivity(Activity.UserGet)]
		public List<UserModel> Get()
		{
			return _userController.Get().ToList();
		}

		[HubAuthorizeActivity(Activity.UserGet)]
		public UserModel Get(Guid id)
		{
			return _userController.Get(id);
		}

		[HubAuthorizeActivity(Activity.UserPost)]
		public UserModel Post(UserDetailModel user)
		{
			var userModel = _userController.Post(user);
			return userModel;
		}

		[HubAuthorizeActivity(Activity.UserUpdate)]
		public UserModel Put(Guid id, UserDetailModel user)
		{
			return _userController.Put(id, user);
		}

		[HubAuthorizeActivity(Activity.UserDelete)]
		public bool Delete(Guid id)
		{
			return _userController.Delete(id);
		}

		[HubAuthorizeActivity(Activity.UserSubscribe)]
		public void OnInsert(UserModel user)
		{
			Clients.All.OnInsert(user);
		}

		[HubAuthorizeActivity(Activity.UserSubscribe)]
		public void OnUpdate(UserModel user)
		{
			Clients.All.OnUpdate(user);
		}

		[HubAuthorizeActivity(Activity.UserSubscribe)]
		public void OnDelete(UserModel user)
		{
			Clients.All.OnDelete(user);
		}

		
	}

	
}