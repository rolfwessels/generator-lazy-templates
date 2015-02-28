using FluentAssertions;
using MainSolutionTemplate.Core.Managers;
using MainSolutionTemplate.Core.MessageUtil;
using MainSolutionTemplate.Core.Tests.Fakes;
using Moq;
using NUnit.Framework;

namespace MainSolutionTemplate.Core.Tests.Managers
{
    [TestFixture]
    public class SystemManagerTests
    {
        protected SystemManager _systemManager;
        public Mock<IMessenger> _mockIMessenger;
        protected FakeGeneralUnitOfWork _fakeGeneralUnitOfWork;

        #region Setup/Teardown

        public virtual void Setup()
        {
            _mockIMessenger = new Mock<IMessenger>();
            _fakeGeneralUnitOfWork = new FakeGeneralUnitOfWork();
            _systemManager = new SystemManager(_fakeGeneralUnitOfWork, _mockIMessenger.Object);
        }

        [TearDown]
        public virtual void TearDown()
        {
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