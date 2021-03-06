using System;
using System.Threading;
using System.Web.Http.Dependencies;
using log4net.Config;
using MainSolutionTemplate.Api.AppStartup;
using MainSolutionTemplate.Api.Models.Mappers;
using MainSolutionTemplate.Api.SignalR;
using MainSolutionTemplate.Api.Swagger;
using MainSolutionTemplate.Api.WebApi;
using Microsoft.Owin;
using Owin;

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
            var webApiSetup = WebApiSetup.Initialize(appBuilder, IocApi.Instance.Resolve<IDependencyResolver>());
            SignalRSetup.Initialize(appBuilder,
                IocApi.Instance.Resolve<Microsoft.AspNet.SignalR.IDependencyResolver>());
            SwaggerSetup.Initialize(webApiSetup.Configuration);
            SimpleFileServer.Initialize(appBuilder);
            webApiSetup.Configuration.EnsureInitialized();
            RegisterAppDisposing(appBuilder, () => { IocApi.Instance.Container.Dispose(); });
        }

        #region Private Methods

        private static void RegisterAppDisposing(IAppBuilder appBuilder, Action callback)
        {
            var context = new OwinContext(appBuilder.Properties);
            var token = context.Get<CancellationToken>("host.OnAppDisposing");
            if (token != CancellationToken.None)
                token.Register(callback);
        }

        #endregion
    }
}