using System.Reflection;
using MainSolutionTemplate.Core.Managers.Interfaces;
using MainSolutionTemplate.Core.MessageUtil;
using MainSolutionTemplate.Dal.Persistance;
using log4net;

namespace MainSolutionTemplate.Core.Managers
{
	public partial class SystemManager : ISystemManager
	{
		private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private readonly IMessenger _messenger;
		private readonly IGeneralUnitOfWork _generalUnitOfWork;

		public SystemManager(IGeneralUnitOfWork generalUnitOfWork, IMessenger messenger)
		{
			_generalUnitOfWork = generalUnitOfWork;
			_messenger = messenger;
		}
	}
}