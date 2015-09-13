using System;
using System.IO;
using System.Reflection;
using MainSolutionTemplate.Api.Properties;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using Owin;
using log4net;

namespace MainSolutionTemplate.Api.AppStartup
{
    public class SimpleFileServer
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public static string PossibleWebBasePath { get; set; }

        static SimpleFileServer()
        {
            PossibleWebBasePath = Path.GetFullPath(Path.Combine(new Uri(Assembly.GetExecutingAssembly().CodeBase).PathAndQuery, @"..\..\..\MainSolutionTemplate.Website\build\debug"));
        }

        public static void Initialize(IAppBuilder appBuilder)
        {
            var webBasePath = Settings.Default.WebBasePath;
            if (!Directory.Exists(webBasePath) && Directory.Exists(PossibleWebBasePath))
            {
                _log.Warn("Using alternative path to base path:" + Path.GetFullPath(PossibleWebBasePath));
                webBasePath = PossibleWebBasePath;
            }
            var options = new FileServerOptions
                {
                    FileSystem = new PhysicalFileSystem(webBasePath)
                };
            appBuilder.UseFileServer(options);
        }
    }
}