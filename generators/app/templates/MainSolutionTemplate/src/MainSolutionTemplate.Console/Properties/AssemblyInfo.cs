using System.Reflection;
using MainSolutionTemplate.Core.Helpers;

[assembly: AssemblyTitle("MainSolutionTemplate.Console")]
[assembly: AssemblyDescription("MainSolutionTemplate console application")]
[assembly: Log4NetInitialize("MainSolutionTemplate.console")]
[assembly: log4net.Config.XmlConfigurator()]