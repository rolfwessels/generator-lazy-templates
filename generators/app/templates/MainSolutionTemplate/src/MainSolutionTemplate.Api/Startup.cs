using System.Web.Http.Dependencies;
using MainSolutionTemplate.Api.AppStartup;
using MainSolutionTemplate.Api.Properties;
using MainSolutionTemplate.Api.SignalR;
using MainSolutionTemplate.Api.Swagger;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using Owin;

namespace MainSolutionTemplate.Api
{
	public class Startup
	{
		public void Configuration(IAppBuilder appBuilder)
		{
			BootStrap.Initialize(appBuilder);
			WebApiSetup webApiSetup = WebApiSetup.Initialize(appBuilder, IocApi.Instance.Resolve<IDependencyResolver>());
			SignalRSetup.Initialize(appBuilder, IocApi.Instance.Resolve<Microsoft.AspNet.SignalR.IDependencyResolver>());
			SwaggerSetup.Initialize(webApiSetup.Configuration);
            SimpleFileServer.Initialize(appBuilder);
		}
	}
}