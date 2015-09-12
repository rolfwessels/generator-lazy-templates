using NUnit.Framework;
using FluentAssertions;

namespace MainSolutionTemplate.Dal.Mongo.Tests
{
    [TestFixture]
    public class MongoDbConnectionFactoryTests
    {

        private MongoDbConnectionFactory _mongoDbConnectionFactory;

        #region Setup/Teardown

        public void Setup()
        {
            _mongoDbConnectionFactory = new MongoDbConnectionFactory();
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
            
            // assert
        }


    }

}