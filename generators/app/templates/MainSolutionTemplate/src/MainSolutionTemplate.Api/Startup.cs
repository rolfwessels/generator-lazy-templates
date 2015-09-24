using System.Web.Http.Dependencies;
using MainSolutionTemplate.Api.AppStartup;
using MainSolutionTemplate.Api.Models.Mappers;
using MainSolutionTemplate.Api.SignalR;
using MainSolutionTemplate.Api.Swagger;
using MainSolutionTemplate.Api.WebApi;
using Owin;
using log4net.Config;

namespace MainSolutionTemplate.Api
{
	public class Startup
	{
	    
	    public void Configuration(IAppBuilder appBuilder)
		{
		    XmlConfigurator.Configure();
	        CrossOrginSetup.UseCors(appBuilder);
			BootStrap.Initialize(appBuilder);
		    MapApi.Initialize();
			WebApiSetup webApiSetup = WebApiSetup.Initialize(appBuilder, IocApi.Instance.Resolve<IDependencyResolver>());
			SignalRSetup.Initialize(appBuilder, IocApi.Instance.Resolve<Microsoft.AspNet.SignalR.IDependencyResolver>());
			SwaggerSetup.Initialize(webApiSetup.Configuration);
            SimpleFileServer.Initialize(appBuilder);
            webApiSetup.Configuration.EnsureInitialized();
		}
	}
}