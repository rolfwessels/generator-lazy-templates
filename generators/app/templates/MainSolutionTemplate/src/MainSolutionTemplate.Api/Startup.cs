using System.Web.Http;
using MainSolutionTemplate.Web.AppStartup;
using Microsoft.Owin.Cors;
using Owin;

namespace MainSolutionTemplate.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
			BootStrap.Initialize();
			WebApiSetup.Initialize(appBuilder);
            

            appBuilder.MapSignalR();
            appBuilder.UseNancy();
        }
    }
}