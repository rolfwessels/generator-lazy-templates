using System.Reflection;
using System.Runtime.InteropServices;
using MainSolutionTemplate.Api;
using MainSolutionTemplate.Core.Helpers;
using Microsoft.Owin;

[assembly: AssemblyTitle("MainSolutionTemplate.Api")]
[assembly: AssemblyDescription("Contains all MainSolutionTemplate unit tests")]
[assembly: ComVisible(false)]
[assembly: Guid("41627d52-98a4-4f2b-adef-0752c128eff9")]
[assembly: Log4NetInitialize("MainSolutionTemplate.Api")]
[assembly: OwinStartup(typeof(Startup))]
