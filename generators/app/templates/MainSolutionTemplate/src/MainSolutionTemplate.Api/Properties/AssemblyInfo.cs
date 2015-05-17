using System.Reflection;
using System.Runtime.InteropServices;
using MainSolutionTemplate.Api;
using Microsoft.Owin;

[assembly: AssemblyTitle("MainSolutionTemplate.Web")]
[assembly: AssemblyDescription("Contains all MainSolutionTemplate unit tests")]

[assembly: OwinStartup(typeof(Startup))]

[assembly: Guid("41627d52-98a4-4f2b-adef-0752c128eff9")]
[assembly: MainSolutionTemplate.Utilities.Helpers.Log4NetInitialize("MainSolutionTemplate.console")]
[assembly: log4net.Config.XmlConfigurator]

