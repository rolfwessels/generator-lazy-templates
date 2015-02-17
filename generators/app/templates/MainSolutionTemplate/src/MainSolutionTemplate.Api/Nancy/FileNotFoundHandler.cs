using System.IO;
using System.Reflection;
using MainSolutionTemplate.Api.Properties;
using Nancy;
using Nancy.ErrorHandling;
using Nancy.ViewEngines;
using log4net;

namespace MainSolutionTemplate.Api.Nancy
{
	public class FileNotFoundHandler : IStatusCodeHandler
	{
		private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private readonly IViewRenderer _viewRenderer;
		private string _viewName;

		public FileNotFoundHandler(IViewRenderer viewRenderer)
		{
			_viewRenderer = viewRenderer;
			_viewName = Path.Combine(Settings.Default.WebBasePath, @"404.html");
		}

		#region IStatusCodeHandler Members

		public bool HandlesStatusCode(HttpStatusCode statusCode,
		                              NancyContext context)
		{
			return statusCode == HttpStatusCode.NotFound;
		}

		public void Handle(HttpStatusCode statusCode, NancyContext context)
		{
			_log.Warn(string.Format("FileNotFoundHandler:Handle Display 404 {0}", _viewName));
			Response response = _viewRenderer.RenderView(context, _viewName);
			response.StatusCode = statusCode;
			context.Response = response;
		}

		#endregion
	}
}