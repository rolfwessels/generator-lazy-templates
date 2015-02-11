using System.Web.UI;
using Autofac;
using Autofac.Integration.WebApi;
using MainSolutionTemplate.Api.WebApi.Controllers;
using MainSolutionTemplate.Core.Startup;
using MainSolutionTemplate.Dal.Mongo;
using MainSolutionTemplate.Dal.Persistance;

namespace MainSolutionTemplate.Api.AppStartup
{
	public class IocContainerSetup : IocContainerBase
	{
		private static bool _isInitialized;
		private static readonly object _locker = new object();
		private static IocContainerSetup _instance;
		private readonly IContainer _container;

		public IocContainerSetup()
		{
			var builder = new ContainerBuilder();
			SetupCore(builder);
			WebApi(builder);
			_container = builder.Build();
			var webApiResolver = new AutofacWebApiDependencyResolver(_container);
			WebApiSetup.Instance.Configuration.DependencyResolver = webApiResolver;
		}

		#region Initialize

		public static IocContainerSetup Instance
		{
			get
			{
				if (_isInitialized) return _instance;
				lock (_locker)
				{
					if (!_isInitialized)
					{
						_instance = new IocContainerSetup();
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

		#region Overrides of IocContainerBase

		protected override IGeneralUnitOfWork GetGeneralUnitOfWork(IComponentContext arg)
		{
			return new MongoGeneralUnitOfWork();
		}

		#endregion

		#region Private Methods

		private static void WebApi(ContainerBuilder builder)
		{
			builder.RegisterApiControllers(typeof(UserController).Assembly);
		}

		#endregion
	}
}