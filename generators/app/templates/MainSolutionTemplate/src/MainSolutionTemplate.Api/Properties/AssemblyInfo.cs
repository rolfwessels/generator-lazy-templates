using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.Owin;

[assembly: AssemblyTitle("MainSolutionTemplate.Web")]
[assembly: AssemblyDescription("Contains all MainSolutionTemplate unit tests")]

[assembly: OwinStartup(typeof(MainSolutionTemplate.Api.Startup))]

[assembly: Guid("41627d52-98a4-4f2b-adef-0752c128eff9")]
[assembly: MainSolutionTemplate.Utilities.Helpers.Log4NetInitialize("MainSolutionTemplate.Api")]
[assembly: log4net.Config.XmlConfigurator]

