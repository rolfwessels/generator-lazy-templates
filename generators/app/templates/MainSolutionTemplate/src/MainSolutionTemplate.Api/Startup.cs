using System.Web.Http.Dependencies;
using MainSolutionTemplate.Api.AppStartup;
using MainSolutionTemplate.Api.SignalR;
using MainSolutionTemplate.Api.Swagger;
using Owin;
using log4net.Config;

namespace MainSolutionTemplate.Api
{
	public class Startup
	{
		public void Configuration(IAppBuilder appBuilder)
		{
		    XmlConfigurator.Configure();
			BootStrap.Initialize(appBuilder);
			WebApiSetup webApiSetup = WebApiSetup.Initialize(appBuilder, IocApi.Instance.Resolve<IDependencyResolver>());
			SignalRSetup.Initialize(appBuilder, IocApi.Instance.Resolve<Microsoft.AspNet.SignalR.IDependencyResolver>());
			SwaggerSetup.Initialize(webApiSetup.Configuration);
            SimpleFileServer.Initialize(appBuilder);
		}
	}
}