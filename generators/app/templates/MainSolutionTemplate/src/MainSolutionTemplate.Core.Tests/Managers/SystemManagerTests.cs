using FluentAssertions;
using MainSolutionTemplate.Core.Managers;
using MainSolutionTemplate.Core.MessageUtil;
using MainSolutionTemplate.Core.Tests.Fakes;
using MainSolutionTemplate.Dal.Validation;
using Moq;
using NUnit.Framework;

namespace MainSolutionTemplate.Core.Tests.Managers
{
    [TestFixture]
    public class SystemManagerTests
    {
        protected SystemManager _systemManager;
        protected FakeGeneralUnitOfWork _fakeGeneralUnitOfWork;
        protected Mock<IMessenger> _mockIMessenger;
        protected Mock<IValidatorFactory> _mockIValidatorFactory;

        #region Setup/Teardown

        public virtual void Setup()
        {
            _mockIMessenger = new Mock<IMessenger>();
            _mockIValidatorFactory = new Mock<IValidatorFactory>();
            _fakeGeneralUnitOfWork = new FakeGeneralUnitOfWork();
            _systemManager = new SystemManager(_fakeGeneralUnitOfWork, _mockIMessenger.Object, _mockIValidatorFactory.Object);
        }

        [TearDown]
        public virtual void TearDown()
        {
            _mockIValidatorFactory.VerifyAll();
            _mockIMessenger.VerifyAll();
        }

        #endregion

        [Test]
        public void Constructor_WhenCalled_ShouldNotFail()
        {
            // arrange
            Setup();
            // assert
            _systemManager.Should().NotBeNull();
        }


    }
}