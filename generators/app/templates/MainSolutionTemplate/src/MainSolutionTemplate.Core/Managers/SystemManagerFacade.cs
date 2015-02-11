using System.Reflection;
using System.Threading.Tasks;
using MainSolutionTemplate.Core.Managers.Interfaces;
using MainSolutionTemplate.Dal.Persistance;
using log4net;

namespace MainSolutionTemplate.Core.Managers
{
	public partial class SystemManagerFacade : ISystemManagerFacade
	{
		private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		private readonly IGeneralUnitOfWork _generalUnitOfWork;

		public SystemManagerFacade(IGeneralUnitOfWork generalUnitOfWork)
		{
			_generalUnitOfWork = generalUnitOfWork;
		}

	}
}