using System.Reflection;

[assembly: AssemblyTitle("MainSolutionTemplate.Core.Tests")]
[assembly: AssemblyDescription("Contains all MainSolutionTemplate Core unit tests.")]
[assembly: MainSolutionTemplate.Core.Helpers.Log4NetInitialize("MainSolutionTemplate.Core.Tests")]
[assembly: log4net.Config.XmlConfigurator()]