using Autofac;
using FluentValidation;
using MainSolutionTemplate.Core.BusinessLogic.Components;
using MainSolutionTemplate.Core.BusinessLogic.Components.Interfaces;
using MainSolutionTemplate.Core.MessageUtil;
using MainSolutionTemplate.Dal.Models;
using MainSolutionTemplate.Dal.Persistance;
using MainSolutionTemplate.Dal.Validation;
using MainSolutionTemplate.OAuth2.Dal.Interfaces;
using IValidatorFactory = MainSolutionTemplate.Dal.Validation.IValidatorFactory;

namespace MainSolutionTemplate.Core.Startup
{
	public abstract class IocCoreBase
	{
		protected void SetupCore(ContainerBuilder builder)
		{
           
            builder.Register(GetGeneralUnitOfWork);
			SetupManagers(builder);
			SetupTools(builder);
            builder.RegisterType<UserValidator>().As<IValidator<User>>();
		}

		private static void SetupManagers(ContainerBuilder builder)
		{
            builder.RegisterType<BaseManagerArguments>();
            builder.RegisterType<ApplicationManager>().As<IApplicationManager>();
            builder.RegisterType<AuthorizeManager>().As<IAuthorizeManager>();
            builder.RegisterType<OAuthDataManager>().As<IOAuthDataManager>();
            builder.RegisterType<ProjectManager>().As<IProjectManager>();
            builder.RegisterType<RoleManager>().As<IRoleManager>();
            builder.RegisterType<UserManager>().As<IUserManager>();

            builder.RegisterType<ValidatorFactory>().As<IValidatorFactory>();
		}

		private void SetupTools(ContainerBuilder builder)
		{
			builder.Register((x) => Messenger.Default).As<IMessenger>();
		}

		protected abstract IGeneralUnitOfWork GetGeneralUnitOfWork(IComponentContext arg);
	}
}
