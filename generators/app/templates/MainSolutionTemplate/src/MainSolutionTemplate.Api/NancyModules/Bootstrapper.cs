using Nancy;
using Nancy.Bootstrapper;
using Nancy.Conventions;
using Nancy.TinyIoc;
using MainSolutionTemplate.Web.AppStartup;

namespace MainSolutionTemplate.Web
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        // The bootstrapper enables you to reconfigure the composition of the framework,
        // by overriding the various methods and properties.
        // For more information https://github.com/NancyFx/Nancy/wiki/Bootstrapper
        protected override void ConfigureConventions(NancyConventions nancyConventions)
        {
            nancyConventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("Content", @"Content"));
            nancyConventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("Scripts", @"Scripts"));
            nancyConventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("Views", @"Views","html"));
            base.ConfigureConventions(nancyConventions);
        }

        #region Overrides of NancyBootstrapperBase<TinyIoCContainer>

        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            AutoMapperSetup.Initialize();
            base.ApplicationStartup(container, pipelines);

        }

        #endregion
    }
}