using Autofac;
using MainSolutionTemplate.Core.Managers;
using MainSolutionTemplate.Core.Managers.Interfaces;
using MainSolutionTemplate.Dal.Persistance;

namespace MainSolutionTemplate.Core.Startup
{
	public abstract class IocContainerBase
	{
		protected void SetupCore(ContainerBuilder builder)
		{
			builder.Register(GetGeneralUnitOfWork).InstancePerLifetimeScope();
			builder.RegisterType<SystemManagerFacade>().As<ISystemManagerFacade>();
			builder.RegisterType<AuthorizeManager>().As<IAuthorizeManager>();
		}

		protected abstract IGeneralUnitOfWork GetGeneralUnitOfWork(IComponentContext arg);
	}

}
