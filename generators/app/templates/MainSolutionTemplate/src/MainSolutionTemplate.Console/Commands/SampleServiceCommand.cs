using System;
using System.Reflection;
using System.ServiceProcess;
using log4net;

namespace MainSolutionTemplate.Console.Commands
{
	public class SampleServiceCommand : ServiceCommandBase
	{
		private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		public SampleServiceCommand()
			: base("MainSolutionTemplateService")
		{
			IsCommand("service", string.Format("Commands to install start and un-install the service"));
		}

		#region Overrides of ServiceCommandBase

		protected override void StartService()
		{
			_log.Info("whhoooopppp");
		}

		protected override void StopService()
		{
			_log.Info("ppppoooohhwww");
		}

		#endregion

		
	}
	
}