using MainSolutionTemplate.Api.AppStartup;
using MainSolutionTemplate.Core.Managers;
using MainSolutionTemplate.Core.Managers.Interfaces;
using MainSolutionTemplate.OAuth2;
using Owin;

namespace MainSolutionTemplate.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            BootStrap.Initialize();
			WebApiSetup webApiSetup = WebApiSetup.Initialize(appBuilder);
			OathAuthorizationSetup.Initialize(appBuilder,IocContainerSetup.Instance.Resolve<ISystemManagerFacade>());
            appBuilder.MapSignalR();
            SwaggerSetup.Initialize(webApiSetup.Configuration);
            appBuilder.UseNancy();
            webApiSetup.Configuration.EnsureInitialized();
        }
    }
}