using System.Linq;
using FluentAssertions;
using MainSolutionTemplate.Utilities.Helpers;
using NUnit.Framework;

namespace MainSolutionTemplate.Utilities.Tests.Helpers
{
    [TestFixture]
    public class ObjectPoolTests
    {
        
        #region Setup/Teardown

        public void Setup()
        {
        }

        [TearDown]
        public void TearDown()
        {
        }

        #endregion

        [Test]
        public void Get_GivenRequestForSameObject_ShouldAlwaysOnlyContainXValues()
        {
            // arrange
            Setup();
            var counter = 0;
            var maxObjects = 2;
            var objectPool = new ObjectPool<Sample>(() => new Sample() { Name = "Name" + counter++ }, maxObjects);
            // action
            var samples = Enumerable.Range(0, 20).Select(x => objectPool.Get()).ToArray();
            // assert
            samples.Select(x => x.Name).Distinct().Should().HaveCount(maxObjects);
        }

        #region Nested type: Sample

        public class Sample
        {
            public string Name { get; set; }
        }

        #endregion
    }
}