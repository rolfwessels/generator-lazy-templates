using System.Web.Http;
using Microsoft.Owin.Cors;
using Owin;

namespace MainSolutionTemplate.Web.AppStartup
{
	public class WebApiSetup
	{
		private static bool _isInitialized;
		private static readonly object _locker = new object();
		private static WebApiSetup _instance;

		protected WebApiSetup(IAppBuilder appBuilder)
		{
			var configuration = new HttpConfiguration();
			appBuilder.UseCors(CorsOptions.AllowAll);
			appBuilder.UseWebApi(configuration);
		}

		#region Initialize

		public static void Initialize(IAppBuilder appBuilder)
		{
			if (_isInitialized) return;
			lock (_locker)
			{
				if (!_isInitialized)
				{
					_instance = new WebApiSetup(appBuilder);
					_isInitialized = true;
				}
			}
		}

		#endregion

	}
}