using System;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.Dependencies;
using System.Web.Http.OData.Extensions;
using MainSolutionTemplate.Api.WebApi.Filters;
using Microsoft.Owin.Cors;
using Newtonsoft.Json.Serialization;
using Owin;

namespace MainSolutionTemplate.Api.AppStartup
{
	public class WebApiSetup
	{
		private static bool _isInitialized;
		private static readonly object _locker = new object();
		private static WebApiSetup _instance;
		private readonly HttpConfiguration _configuration;

		protected WebApiSetup(IAppBuilder appBuilder, IDependencyResolver dependencyResolver)
		{
			var configuration = new HttpConfiguration();
			configuration.AddODataQueryFilter();

			SetupRoutes(configuration);
			SetupGlobalFilters(configuration);
			SetApiCamelCase(configuration);
			appBuilder.UseCors(CorsOptions.AllowAll);
			appBuilder.UseWebApi(configuration);
			configuration.DependencyResolver = dependencyResolver;
			_configuration = configuration;
		}

		private static void SetApiCamelCase(HttpConfiguration configuration)
		{
			var jsonFormatter = configuration.Formatters.OfType<JsonMediaTypeFormatter>().First();
			jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
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

		public static WebApiSetup Initialize(IAppBuilder appBuilder, IDependencyResolver dependencyResolver)
		{
			if (_isInitialized) return _instance;
			lock (_locker)
			{
				if (!_isInitialized)
				{
					_instance = new WebApiSetup(appBuilder,dependencyResolver);
					_isInitialized = true;
				}
			}
			return _instance;
		}

		#endregion
	}
}