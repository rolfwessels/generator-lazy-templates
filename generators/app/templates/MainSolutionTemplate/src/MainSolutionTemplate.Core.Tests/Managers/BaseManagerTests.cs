using FluentAssertions;
using MainSolutionTemplate.Core.BusinessLogic.Components;
using MainSolutionTemplate.Core.MessageUtil;
using MainSolutionTemplate.Core.Tests.Fakes;
using MainSolutionTemplate.Dal.Validation;
using Moq;
using NUnit.Framework;

namespace MainSolutionTemplate.Core.Tests.Managers
{
    [TestFixture]
    public class BaseManagerTests
    {
        
        protected FakeGeneralUnitOfWork _fakeGeneralUnitOfWork;
        protected Mock<IMessenger> _mockIMessenger;
        protected Mock<IValidatorFactory> _mockIValidatorFactory;
        protected BaseManagerArguments _baseManagerArguments;

        #region Setup/Teardown

        public virtual void Setup()
        {
            _mockIMessenger = new Mock<IMessenger>();
            _mockIValidatorFactory = new Mock<IValidatorFactory>();
            _fakeGeneralUnitOfWork = new FakeGeneralUnitOfWork();
            _baseManagerArguments = new BaseManagerArguments(_fakeGeneralUnitOfWork, _mockIMessenger.Object, _mockIValidatorFactory.Object);
        }

        [TearDown]
        public virtual void TearDown()
        {
            _mockIValidatorFactory.VerifyAll();
            _mockIMessenger.VerifyAll();
        }

        #endregion


    }
}