using System.Web.Http.Dependencies;
using MainSolutionTemplate.Api.AppStartup;
using MainSolutionTemplate.Api.Models.Mappers;
using MainSolutionTemplate.Api.SignalR;
using MainSolutionTemplate.Api.Swagger;
using Owin;
using log4net.Config;

namespace MainSolutionTemplate.Api
{
	public class Startup
	{
	    private readonly CrossOrginSetupp _crossOrginSetupp = new CrossOrginSetupp();

	    public void Configuration(IAppBuilder appBuilder)
		{
		    XmlConfigurator.Configure();
	        CrossOrginSetupp.UseCors(appBuilder);
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