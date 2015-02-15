using MainSolutionTemplate.Core.Managers;
using MainSolutionTemplate.Dal.Persistance;
using Moq;
using NUnit.Framework;
using FluentAssertions;

namespace MainSolutionTemplate.Core.Tests
{
	[TestFixture]
	public class SystemManagerFacadeUserManagerTests
	{
		private SystemManagerFacade _systemManagerFacadeUserManager;
		private Mock<IGeneralUnitOfWork> _mockIGeneralUnitOfWork;

		#region Setup/Teardown

		public void Setup()
		{
			_mockIGeneralUnitOfWork = new Mock<IGeneralUnitOfWork>(MockBehavior.Strict);
			
			_systemManagerFacadeUserManager = new SystemManagerFacade(_mockIGeneralUnitOfWork.Object);
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
			_systemManagerFacadeUserManager.Should().NotBeNull();
		}

		 
	}

}