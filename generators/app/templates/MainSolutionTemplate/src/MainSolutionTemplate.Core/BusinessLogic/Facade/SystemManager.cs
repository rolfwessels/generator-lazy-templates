using System.Reflection;
using MainSolutionTemplate.Core.BusinessLogic.Facade.Interfaces;
using MainSolutionTemplate.Core.MessageUtil;
using MainSolutionTemplate.Dal.Persistance;
using MainSolutionTemplate.Dal.Validation;
using log4net;

namespace MainSolutionTemplate.Core.BusinessLogic.Facade
{
	public partial class SystemManager : ISystemManager
	{
		private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private readonly IMessenger _messenger;
		private readonly IGeneralUnitOfWork _generalUnitOfWork;
        private readonly IValidatorFactory _validationFactory;

	    public SystemManager(IGeneralUnitOfWork generalUnitOfWork, IMessenger messenger, IValidatorFactory validationFactory)
		{
			_generalUnitOfWork = generalUnitOfWork;
			_messenger = messenger;
	        _validationFactory = validationFactory;
		}
	}
}