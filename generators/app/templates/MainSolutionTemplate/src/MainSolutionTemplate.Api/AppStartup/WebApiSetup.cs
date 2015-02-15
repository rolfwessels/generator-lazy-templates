using System;
using System.Web.Http;
using System.Web.Http.OData.Extensions;
using MainSolutionTemplate.Api.WebApi.Filters;
using Microsoft.Owin.Cors;
using Owin;

namespace MainSolutionTemplate.Api.AppStartup
{
	public class WebApiSetup
	{
		private static bool _isInitialized;
		private static readonly object _locker = new object();
		private static WebApiSetup _instance;
		private readonly HttpConfiguration _configuration;

		protected WebApiSetup(IAppBuilder appBuilder)
		{
			var configuration = new HttpConfiguration();
			configuration.AddODataQueryFilter();

			appBuilder.UseCors(CorsOptions.AllowAll);
			SetupRoutes(configuration);
			appBuilder.UseWebApi(configuration);
			SetupGlobalFilters(configuration);
			_configuration = configuration;
		}

		public HttpConfiguration Configuration
		{
			get { return _configuration; }
		}

		public static WebApiSetup Instance
		{
			get
			{
				if (_instance == null) throw new Exception("Call Initialize before using Intance.");
				return _instance;
			}
		}

		#region Private Methods

		private static void SetupRoutes(HttpConfiguration configuration)
		{
			configuration.MapHttpAttributeRoutes();
		}

		private static void SetupGlobalFilters(HttpConfiguration configuration)
		{
			configuration.Filters.Add(new CaptureExceptionFilter());
		}

		#endregion

		#region Initialize

		public static WebApiSetup Initialize(IAppBuilder appBuilder)
		{
			if (_isInitialized) return _instance;
			lock (_locker)
			{
				if (!_isInitialized)
				{
					_instance = new WebApiSetup(appBuilder);
					_isInitialized = true;
				}
			}
			return _instance;
		}

		#endregion
	}
}