using System.Reflection;

[assembly: AssemblyTitle("MainSolutionTemplate.Website.Tests")]
[assembly: AssemblyDescription("Contains all MainSolutionTemplate unit tests")]
[assembly: MainSolutionTemplate.Core.Helpers.Log4NetInitialize("MainSolutionTemplate.Website.Tests")]
[assembly: log4net.Config.XmlConfigurator()]