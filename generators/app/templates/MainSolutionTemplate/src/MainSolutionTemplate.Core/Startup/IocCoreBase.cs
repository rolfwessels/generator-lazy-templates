using System;
using System.Linq;
using Autofac;
using FluentValidation;
using MainSolutionTemplate.Core.Managers;
using MainSolutionTemplate.Core.Managers.Interfaces;
using MainSolutionTemplate.Core.MessageUtil;
using MainSolutionTemplate.Dal.Models;
using MainSolutionTemplate.Dal.Persistance;
using MainSolutionTemplate.Dal.Validation;
using IValidatorFactory = MainSolutionTemplate.Dal.Validation.IValidatorFactory;

namespace MainSolutionTemplate.Core.Startup
{
	public abstract class IocCoreBase
	{
		protected void SetupCore(ContainerBuilder builder)
		{
			builder.Register(GetGeneralUnitOfWork).InstancePerLifetimeScope();
			SetupManagers(builder);
			SetupTools(builder);
            builder.RegisterType<UserValidator>().As<IValidator<User>>();
//            builder.RegisterAssemblyTypes(typeof(UserValidator).Assembly)
//            .Where(t => t.IsSubclassOf(typeof(IValidator<>)))
//            .As(typeof(IValidator<>))
//            .InstancePerDependency();

//		    builder.RegisterGeneric(typeof (AbstractValidator<>))
//		           .As(typeof (IValidator<>))
//		           .InstancePerDependency();
		}

		private static void SetupManagers(ContainerBuilder builder)
		{
			builder.RegisterType<SystemManager>().As<ISystemManager>();
			builder.RegisterType<AuthorizeManager>().As<IAuthorizeManager>();

            builder.RegisterType<ValidatorFactory>().As<IValidatorFactory>();
		}

		private void SetupTools(ContainerBuilder builder)
		{
			builder.Register((x) => Messenger.Default).As<IMessenger>();
		}

		protected abstract IGeneralUnitOfWork GetGeneralUnitOfWork(IComponentContext arg);
	}
}
