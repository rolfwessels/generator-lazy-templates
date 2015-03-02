using Autofac;
using MainSolutionTemplate.Core.Managers;
using MainSolutionTemplate.Core.Managers.Interfaces;
using MainSolutionTemplate.Core.MessageUtil;
using MainSolutionTemplate.Dal.Persistance;

namespace MainSolutionTemplate.Core.Startup
{
	public abstract class IocCoreBase
	{
		protected void SetupCore(ContainerBuilder builder)
		{
			builder.Register(GetGeneralUnitOfWork).InstancePerLifetimeScope();
			SetupManagers(builder);
			SetupTools(builder);
		}

		private static void SetupManagers(ContainerBuilder builder)
		{
			builder.RegisterType<SystemManager>().As<ISystemManager>();
			builder.RegisterType<AuthorizeManager>().As<IAuthorizeManager>();
		}

		private void SetupTools(ContainerBuilder builder)
		{
			builder.Register((x) => Messenger.Default).As<IMessenger>();
		}

		protected abstract IGeneralUnitOfWork GetGeneralUnitOfWork(IComponentContext arg);
	}

}
