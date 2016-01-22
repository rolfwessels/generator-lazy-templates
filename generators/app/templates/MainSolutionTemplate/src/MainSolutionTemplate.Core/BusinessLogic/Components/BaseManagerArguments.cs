using MainSolutionTemplate.Core.DataIntegrity;
using MainSolutionTemplate.Core.MessageUtil;
using MainSolutionTemplate.Dal.Persistance;
using MainSolutionTemplate.Dal.Validation;

namespace MainSolutionTemplate.Core.BusinessLogic.Components
{
    public class BaseManagerArguments
    {
        protected readonly IGeneralUnitOfWork _generalUnitOfWork;
        protected readonly IMessenger _messenger;
        protected readonly IValidatorFactory _validationFactory;
        protected readonly IDataIntegrityManager _dataIntegrityManager;

        public BaseManagerArguments(IGeneralUnitOfWork generalUnitOfWork, IMessenger messenger,
                                    IValidatorFactory validationFactory)
        {
            _generalUnitOfWork = generalUnitOfWork;
            _messenger = messenger;
            _validationFactory = validationFactory;
            _dataIntegrityManager = new DataIntegrityManager(_generalUnitOfWork, IntegrityOperators.Default);
        }

        public IGeneralUnitOfWork GeneralUnitOfWork
        {
            get { return _generalUnitOfWork; }
        }

        public IMessenger Messenger
        {
            get { return _messenger; }
        }

        public IValidatorFactory ValidationFactory
        {
            get { return _validationFactory; }
        }

        public IDataIntegrityManager DataIntegrityManager
        {
            get { return _dataIntegrityManager; }
        }
    }
}