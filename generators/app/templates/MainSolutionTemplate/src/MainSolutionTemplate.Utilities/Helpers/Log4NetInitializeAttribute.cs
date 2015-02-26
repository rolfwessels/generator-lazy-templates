using System;
using System.Reflection;

namespace MainSolutionTemplate.Utilities.Helpers
{
	public class Log4NetInitializeAttribute : Attribute
	{
		public Log4NetInitializeAttribute(string name = null)
		{
			log4net.GlobalContext.Properties["machineName"] = Environment.MachineName;
			log4net.GlobalContext.Properties["assemblyName"] = name??(Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly()).GetName().Name;
		}
	}
}