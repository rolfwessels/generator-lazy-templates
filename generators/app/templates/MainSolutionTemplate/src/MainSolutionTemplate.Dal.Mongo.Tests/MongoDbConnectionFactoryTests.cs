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
        public void DatabaseName_GivenConnection_ShouldBeCorrect()
        {
            // arrange
            Setup();
            // action
            var databaseName = _mongoDbConnectionFactory.DatabaseName;
            // assert
            databaseName.Should().Be("MainSolutionTemplate_Develop");
        }


        [Test]
        public void ConnectionString_GivenConnection_ShouldBeCorrect()
        {
            // arrange
            Setup();
            // action
            var databaseName = _mongoDbConnectionFactory.ConnectionString;
            // assert
            databaseName.Should().Be(Settings.Default.Connection);
        }



        [Test]
        public void Constructor_WhenCalled_ShouldNotBeNull()
        {
            // arrange
            Setup();
            // assert
            _mongoDbConnectionFactory.Should().NotBeNull();
        }

        [Test]
        public void GetConnection_WhenCalled_ShouldReturnNewConnection()
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