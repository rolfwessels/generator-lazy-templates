using System;
using System.Reflection;
using MainSolutionTemplate.Api.SignalR.Modules;
using MainSolutionTemplate.Shared.Models;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Owin;

namespace MainSolutionTemplate.Api.SignalR
{
	public class SignalRSetup
	{
		private static SignalRSetup _instance = null;
		private static readonly object _locker = new object();

		private SignalRSetup(IAppBuilder appBuilder, IDependencyResolver resolve)
		{
			GlobalHost.DependencyResolver = resolve;
	        appBuilder.MapSignalR(new HubConfiguration { EnableDetailedErrors = true });
		    GlobalHost.HubPipeline.AddModule(new HubErrorModule());

		}

		#region Instance

		public static SignalRSetup Initialize(IAppBuilder appBuilder, IDependencyResolver resolve)
		{
			lock (_locker)
			{
				if (_instance == null)
				{
					_instance = new SignalRSetup(appBuilder, resolve);
				}
			}
			return _instance;
		}

		#endregion

	}

  
}