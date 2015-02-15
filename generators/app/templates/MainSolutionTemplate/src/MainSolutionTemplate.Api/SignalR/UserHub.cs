using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MainSolutionTemplate.Api.AppStartup;
using MainSolutionTemplate.Api.Models;
using MainSolutionTemplate.Api.Models.Mappers;
using MainSolutionTemplate.Core.Managers.Interfaces;
using Microsoft.AspNet.SignalR;
using log4net;

namespace MainSolutionTemplate.Api.SignalR
{
	public interface IUserHub
	{
		List<UserModel> Get();
	}

	public class UserHub : Hub, IUserHub
	{
		private readonly ISystemManagerFacade _systemManagerFacade;
		private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		public UserHub()
		{
			_systemManagerFacade = IocContainerSetup.Instance.Resolve<ISystemManagerFacade>();
		}

		public List<UserModel> Get()
		{
			var transport = Context.QueryString["transport"];
			_log.Debug(string.Format("UserHub:Get Transport {0}", transport));
			
			return _systemManagerFacade.GetUsers().ToUserModel().ToList();
			//Clients.All.addMessage(data, message);
		}
	}
}