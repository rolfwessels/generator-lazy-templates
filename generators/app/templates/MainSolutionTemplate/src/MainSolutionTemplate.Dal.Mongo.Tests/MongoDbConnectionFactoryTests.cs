using MainSolutionTemplate.Dal.Mongo.Properties;
using NUnit.Framework;
using FluentAssertions;

namespace MainSolutionTemplate.Dal.Mongo.Tests
{
    [TestFixture]
    public class MongoDbConnectionFactoryTests
    {

        private MongoConnectionFactory _mongoDbConnectionFactory;

        #region Setup/Teardown

        public void Setup()
        {
            _mongoDbConnectionFactory = new MongoConnectionFactory(Settings.Default.Connection);
        }

        [TearDown]
        public void TearDown()
        {

        }

        #endregion

        [Test]
        public void Constructor_WhenCalled_ShouldNotBeNull()
        {
            // arrange
            Setup();
            // assert
            _mongoDbConnectionFactory.Should().NotBeNull();
        }

        [Test]
        public void GetConnnection_WhenCalled_ShouldReturnNewConnection()
        {
            // arrange
            Setup();
            // action
            var generalUnitOfWork = _mongoDbConnectionFactory.GetConnection();
            // assert
            generalUnitOfWork.Should().NotBeNull();
        }


    }

}