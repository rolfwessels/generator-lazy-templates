﻿using System.Web.Http.Dependencies;
using Autofac;
using Autofac.Integration.SignalR;
using Autofac.Integration.WebApi;
using MainSolutionTemplate.Api.Common;
using MainSolutionTemplate.Api.Properties;
using MainSolutionTemplate.Api.SignalR.Connection;
using MainSolutionTemplate.Api.SignalR.Hubs;
using MainSolutionTemplate.Api.WebApi.Controllers;
using MainSolutionTemplate.Core.Startup;
using MainSolutionTemplate.Dal.Mongo;
using MainSolutionTemplate.Dal.Persistance;

namespace MainSolutionTemplate.Api.AppStartup
{
	public class IocApi : IocCoreBase
	{
		private static bool _isInitialized;
		private static readonly object _locker = new object();
		private static IocApi _instance;
		private readonly IContainer _container;

	    public IocApi()
		{
		    
	        var builder = new ContainerBuilder();
			SetupCore(builder);
			SetupSignalr(builder);
            SetupCommonControllers(builder);
		    SetupTools(builder);
			WebApi(builder);
			SignalRHubs(builder);
			_container = builder.Build();
		    
		}


	    private void SetupSignalr(ContainerBuilder builder)
		{
			builder.RegisterType<ConnectionStateMapping>().As<IConnectionStateMapping>().SingleInstance();
		}

		private void SetupCommonControllers(ContainerBuilder builder)
		{
			builder.RegisterType<UserCommonController>();
			builder.RegisterType<ProjectCommonController>();
		}

        private void SetupTools(ContainerBuilder builder)
		{
            
		}

		#region Instance

		public static IocApi Instance
		{
			get
			{
				if (_isInitialized) return _instance;
				lock (_locker)
				{
					if (!_isInitialized)
					{
						_instance = new IocApi();
						_isInitialized = true;
					}
				}
				return _instance;
			}
		}

		public IContainer Container
		{
			get { return _container; }
		}


		public T Resolve<T>()
		{
			return _container.Resolve<T>();
		}

		#endregion

		#region Overrides of IocCoreBase

        protected override IGeneralUnitOfWorkFactory GetInstanceOfIGeneralUnitOfWorkFactory(IComponentContext arg)
		{
            return new MongoConnectionFactory(Dal.Mongo.Properties.Settings.Default.Connection);
		}

		#endregion

		#region Private Methods

		private void WebApi(ContainerBuilder builder)
		{
			builder.RegisterApiControllers(typeof(UserController).Assembly);
			builder.Register(t => new AutofacWebApiDependencyResolver(_container)).As<IDependencyResolver>();
		}

		private void SignalRHubs(ContainerBuilder builder)
		{
            builder.RegisterHubs(typeof(NotificationHub).Assembly);
			builder.Register(t => new AutofacDependencyResolver(_container)).As<Microsoft.AspNet.SignalR.IDependencyResolver>();
		}

		#endregion
	}
}