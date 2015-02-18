using System.IO;
using System.Reflection;
using MainSolutionTemplate.Api.Properties;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Conventions;
using Nancy.TinyIoc;
using System.Linq;
using log4net;

namespace MainSolutionTemplate.Api.Nancy
{
	public class NancySetup : DefaultNancyBootstrapper
	{
		private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		protected override void ConfigureConventions(NancyConventions nancyConventions)
		{
			nancyConventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("assets", GetPath("images")));
			nancyConventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("styles", GetPath("styles")));
			nancyConventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("scripts", GetPath("scripts"), "js"));
			nancyConventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("views", GetPath("views"), "html"));
//			nancyConventions.StaticContentsConventions.AddFile("robots.txt",GetPath("robots.txt"));
//			nancyConventions.StaticContentsConventions.AddFile("favicon.ico", GetPath("favicon.ico"));
			base.ConfigureConventions(nancyConventions);
		}

		#region Overrides of NancyBootstrapperBase<TinyIoCContainer>

		protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
		{
			Conventions.ViewLocationConventions.Add((viewName, model, context) => { return GetPath(viewName+".html"); });
			base.ApplicationStartup(container, pipelines);
		}

		#endregion

		private string GetPath(params string[] paths)
		{
			var strings = new[] {Settings.Default.WebBasePath}.Union(paths).ToArray();
			var combine = Path.Combine(strings);
			_log.Info(string.Format("View paths: {0}", combine));
			return combine;
		}
	}
}