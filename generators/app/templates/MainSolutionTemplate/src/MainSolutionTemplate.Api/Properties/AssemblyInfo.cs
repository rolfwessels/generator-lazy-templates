using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.Owin;
using MainSolutionTemplate.Web;

[assembly: AssemblyTitle("MainSolutionTemplate.Web")]
[assembly: AssemblyDescription("Contains all MainSolutionTemplate unit tests")]
[assembly: log4net.Config.XmlConfigurator(Watch = true)]
[assembly: OwinStartup(typeof(Startup))]

[assembly: ComVisible(false)]
[assembly: Guid("41627d52-98a4-4f2b-adef-0752c128eff9")]

