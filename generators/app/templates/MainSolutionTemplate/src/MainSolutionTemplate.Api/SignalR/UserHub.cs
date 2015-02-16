using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MainSolutionTemplate.Api.AppStartup;
using MainSolutionTemplate.Api.Models;
using MainSolutionTemplate.Api.WebApi.Controllers;
using log4net;

namespace MainSolutionTemplate.Api.SignalR
{
	[QueryStringBearerAuthorize]
	public class UserHub : BaseHub , IUserHub
	{
		private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private UserController _userController;


		public UserHub()
		{
			_userController = IocContainerSetup.Instance.Resolve<UserController>();
		}

		public List<UserModel> Get()
		{
			return _userController.Get().ToList();
		}

		public UserModel Get(Guid id)
		{
			return _userController.Get(id);
		}

		public UserModel Post(UserModel user)
		{
			var userModel = _userController.Post(user);
			return userModel;
		}

		public UserModel Put(Guid id, UserModel user)
		{
			return _userController.Put(id, user);
		}

		public bool Delete(Guid id)
		{
			return _userController.Delete(id);
		}
	}

	
}