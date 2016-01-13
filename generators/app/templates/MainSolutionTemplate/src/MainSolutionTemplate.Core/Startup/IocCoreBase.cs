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
            SetupValidation(builder);
		}

	    private static void SetupManagers(ContainerBuilder builder)
		{
            builder.RegisterType<BaseManagerArguments>();
            builder.RegisterType<ApplicationManager>().As<IApplicationManager>();
            builder.RegisterType<OAuthDataManager>().As<IOAuthDataManager>();
            builder.RegisterType<ProjectManager>().As<IProjectManager>();
            builder.RegisterType<RoleManager>().As<IRoleManager>();
            builder.RegisterType<UserManager>().As<IUserManager>();

            
		}

	    private static void SetupValidation(ContainerBuilder builder)
	    {
            builder.RegisterType<ValidatorFactory>().As<IValidatorFactory>();
	        builder.RegisterType<UserValidator>().As<IValidator<User>>();
	        builder.RegisterType<ProjectValidator>().As<IValidator<Project>>();
	        builder.RegisterType<UserValidator>().As<IValidator<User>>();
	    }

	    private void SetupTools(ContainerBuilder builder)
		{
			builder.Register((x) => Messenger.Default).As<IMessenger>();
		}

		protected abstract IGeneralUnitOfWork GetGeneralUnitOfWork(IComponentContext arg);
	}
}
