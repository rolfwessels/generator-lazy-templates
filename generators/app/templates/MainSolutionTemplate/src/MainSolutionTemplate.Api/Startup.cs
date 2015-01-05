using MainSolutionTemplate.Api.AppStartup;
using Owin;

namespace MainSolutionTemplate.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            BootStrap.Initialize();
            WebApiSetup webApiSetup = WebApiSetup.Initialize(appBuilder);
            appBuilder.MapSignalR();
            appBuilder.UseNancy();
            webApiSetup.Configuration.EnsureInitialized();
        }
    }
}