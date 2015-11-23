using FluentAssertions;
using NUnit.Framework;

namespace MainSolutionTemplate.Dal.Mongo.Tests
{
    [TestFixture]
    public class MongoDbCountersTest
    {

        private DataCounter _dataCounter;

        #region Setup/Teardown

        public void Setup()
        {
            _dataCounter = new DataCounter("Test");

        }

        #endregion

       
        
        [Test]
        public void AddGet_WhenCalled_ShouldIncreaseGet()
        {
            // arrange
            Setup();
            _dataCounter.AddGet();
            // assert
            _dataCounter.GetCounter.Should().Be(1);
        }  

        [Test]
        public void AddDelete_WhenCalled_ShouldIncreaseDelete()
        {
            // arrange
            Setup();
            _dataCounter.AddDelete();
            // assert
            _dataCounter.DeleteCounter.Should().Be(1);
        }
 
      
 
        [Test]
        public void AddUpdate_WhenCalled_ShouldIncreaseUpdate()
        {
            // arrange
            Setup();
            _dataCounter.AddUpdate();
            // assert
            _dataCounter.UpdateCounter.Should().Be(1);
        }

        [Test]
        public void AddInsert_WhenCalled_ShouldIncreaseInsert()
        {
            // arrange
            Setup();
            _dataCounter.AddInsert();
            // assert
            _dataCounter.InsertCounter.Should().Be(1);
        }

       

         
    }
}