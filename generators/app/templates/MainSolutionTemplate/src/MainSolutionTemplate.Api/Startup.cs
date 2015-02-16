using MainSolutionTemplate.Api.AppStartup;
using MainSolutionTemplate.Core.Managers.Interfaces;
using MainSolutionTemplate.OAuth2;
using Microsoft.AspNet.SignalR;
using Owin;
using IDependencyResolver = System.Web.Http.Dependencies.IDependencyResolver;

namespace MainSolutionTemplate.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            BootStrap.Initialize();
			OathAuthorizationSetup.Initialize(appBuilder, IocContainerSetup.Instance.Resolve<ISystemManagerFacade>());
			WebApiSetup webApiSetup = WebApiSetup.Initialize(appBuilder , IocContainerSetup.Instance.Resolve<IDependencyResolver>());
			

	        appBuilder.MapSignalR(new HubConfiguration { EnableDetailedErrors = true });
            SwaggerSetup.Initialize(webApiSetup.Configuration);
            appBuilder.UseNancy();
            webApiSetup.Configuration.EnsureInitialized();
        }
    }
}