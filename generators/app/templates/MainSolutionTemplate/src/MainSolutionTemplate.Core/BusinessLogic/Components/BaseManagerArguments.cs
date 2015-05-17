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

        public BaseManagerArguments(IGeneralUnitOfWork generalUnitOfWork, IMessenger messenger,
                                       IValidatorFactory validationFactory)
        {
            _generalUnitOfWork = generalUnitOfWork;
            _messenger = messenger;
            _validationFactory = validationFactory;
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
    }
}