using Autofac;
using MainSolutionTemplate.Core.Managers;
using MainSolutionTemplate.Dal.Persistance;
using MainSolutionTemplate.OAuth2.Dal.Interfaces;
using log4net.Core;

namespace MainSolutionTemplate.Core.Startup
{
	public abstract class IocContainerBase
	{
		protected void SetupCore(ContainerBuilder builder)
		{
			builder.Register(GetGeneralUnitOfWork).InstancePerLifetimeScope();
			builder.RegisterType<SystemManagerFacade>().As<ISystemManagerFacade>();
			builder.RegisterType<OAuthDataManager>().As<IOAuthDataManager>();
		}

		protected abstract IGeneralUnitOfWork GetGeneralUnitOfWork(IComponentContext arg);
	}

}
