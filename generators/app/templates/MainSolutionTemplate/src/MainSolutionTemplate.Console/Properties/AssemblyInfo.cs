using System.Reflection;

[assembly: AssemblyTitle("MainSolutionTemplate.Console")]
[assembly: AssemblyDescription("MainSolutionTemplate console application")]
[assembly: MainSolutionTemplate.Core.Helpers.Log4NetInitialize("MainSolutionTemplate.console")]
[assembly: log4net.Config.XmlConfigurator()]