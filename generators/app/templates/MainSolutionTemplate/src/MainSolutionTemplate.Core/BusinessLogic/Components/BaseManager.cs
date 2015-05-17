using MainSolutionTemplate.Core.MessageUtil;
using MainSolutionTemplate.Dal.Persistance;
using MainSolutionTemplate.Dal.Validation;

namespace MainSolutionTemplate.Core.BusinessLogic.Components
{
    public abstract class BaseManager
    {
        protected readonly IGeneralUnitOfWork _generalUnitOfWork;
        protected readonly IMessenger _messenger;
        protected readonly IValidatorFactory _validationFactory;

        protected BaseManager(BaseManagerArguments baseManagerArguments)
        {
            _generalUnitOfWork = baseManagerArguments.GeneralUnitOfWork;
            _messenger = baseManagerArguments.Messenger;
            _validationFactory = baseManagerArguments.ValidationFactory;
        }
    }
}