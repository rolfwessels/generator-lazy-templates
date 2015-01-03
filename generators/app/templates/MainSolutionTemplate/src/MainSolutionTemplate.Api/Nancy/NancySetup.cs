using Nancy;
using Nancy.Bootstrapper;
using Nancy.Conventions;
using Nancy.TinyIoc;

namespace MainSolutionTemplate.Api.Nancy
{
    public class NancySetup : DefaultNancyBootstrapper
    {
        // The bootstrapper enables you to reconfigure the composition of the framework,
        // by overriding the various methods and properties.
        // For more information https://github.com/NancyFx/Nancy/wiki/Bootstrapper
        protected override void ConfigureConventions(NancyConventions nancyConventions)
        {
			nancyConventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("Content", @"Nancy\Content"));
			nancyConventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("Scripts", @"Nancy\Scripts"));
			nancyConventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("Views", @"Nancy\Views", "html"));
            base.ConfigureConventions(nancyConventions);
        }

        #region Overrides of NancyBootstrapperBase<TinyIoCContainer>

        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
			this.Conventions.ViewLocationConventions.Add((viewName, model, context) =>
			{
				return string.Concat("Nancy/Views/", viewName);
			});
            base.ApplicationStartup(container, pipelines);

        }

        #endregion
    }
}