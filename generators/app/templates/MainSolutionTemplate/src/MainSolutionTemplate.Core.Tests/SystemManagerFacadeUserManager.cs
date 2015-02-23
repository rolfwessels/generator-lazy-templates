using MainSolutionTemplate.Core.Managers;
using MainSolutionTemplate.Core.MessageUtil;
using MainSolutionTemplate.Dal.Persistance;
using Moq;
using NUnit.Framework;
using FluentAssertions;

namespace MainSolutionTemplate.Core.Tests
{
	[TestFixture]
	public class SystemManagerFacadeUserManagerTests
	{
		private SystemManager _systemManagerUserManager;
		private Mock<IGeneralUnitOfWork> _mockIGeneralUnitOfWork;

		#region Setup/Teardown

		public void Setup()
		{
			_mockIGeneralUnitOfWork = new Mock<IGeneralUnitOfWork>(MockBehavior.Strict);
			
			_systemManagerUserManager = new SystemManager(_mockIGeneralUnitOfWork.Object, Messenger.Default);
		}

		[TearDown]
		public void TearDown()
		{

			_mockIGeneralUnitOfWork.VerifyAll();
		}

		#endregion

		[Test]
		public void Constructor_WhenCalled_ShouldNotBeNull()
		{
			// arrange
			Setup();
			// assert
			_systemManagerUserManager.Should().NotBeNull();
		}

		 
	}

}