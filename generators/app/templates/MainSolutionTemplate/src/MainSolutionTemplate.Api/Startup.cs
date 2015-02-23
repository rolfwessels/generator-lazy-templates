using System.IO;
using System.Reflection;
using MainSolutionTemplate.Api.AppStartup;
using MainSolutionTemplate.Api.Properties;
using MainSolutionTemplate.Core.Managers.Interfaces;
using MainSolutionTemplate.OAuth2;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using Owin;
using log4net;
using log4net.Config;
using IDependencyResolver = System.Web.Http.Dependencies.IDependencyResolver;

namespace MainSolutionTemplate.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
	        XmlConfigurator.Configure();
			BootStrap.Initialize(appBuilder);
	        
			
			WebApiSetup webApiSetup = WebApiSetup.Initialize(appBuilder , IocApi.Instance.Resolve<IDependencyResolver>());
			SignalRSetup.Initialize(appBuilder,IocApi.Instance.Resolve<Microsoft.AspNet.SignalR.IDependencyResolver>());
            SwaggerSetup.Initialize(webApiSetup.Configuration);
			var options = new FileServerOptions
			{
				FileSystem = new PhysicalFileSystem(Settings.Default.WebBasePath)
			};
	        appBuilder.UseFileServer(options);
        }
    }
}
