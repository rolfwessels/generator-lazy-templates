using System.Reflection;

[assembly: AssemblyTitle("MainSolutionTemplate.Shared.Tests")]
[assembly: AssemblyDescription("Contains all MainSolutionTemplate unit tests for the shared components")]
[assembly: MainSolutionTemplate.Core.Helpers.Log4NetInitialize("MainSolutionTemplate.Shared.Tests")]
[assembly: log4net.Config.XmlConfigurator()]