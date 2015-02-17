using System.IO;
using MainSolutionTemplate.Api.AppStartup;
using MainSolutionTemplate.Core.Managers.Interfaces;
using MainSolutionTemplate.OAuth2;
using Microsoft.AspNet.SignalR;
using Owin;
using log4net.Config;
using IDependencyResolver = System.Web.Http.Dependencies.IDependencyResolver;

namespace MainSolutionTemplate.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
	        XmlConfigurator.Configure();
            BootStrap.Initialize();
			OathAuthorizationSetup.Initialize(appBuilder, IocContainerSetup.Instance.Resolve<ISystemManagerFacade>());
			WebApiSetup webApiSetup = WebApiSetup.Initialize(appBuilder , IocContainerSetup.Instance.Resolve<IDependencyResolver>());
			SignalRSetup.Initialize(appBuilder,IocContainerSetup.Instance.Resolve<Microsoft.AspNet.SignalR.IDependencyResolver>());
            SwaggerSetup.Initialize(webApiSetup.Configuration);
            appBuilder.UseNancy();
            webApiSetup.Configuration.EnsureInitialized();
        }
    }
}