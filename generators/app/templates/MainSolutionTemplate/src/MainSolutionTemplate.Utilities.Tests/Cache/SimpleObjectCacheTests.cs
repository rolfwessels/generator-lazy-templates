using System;
using FluentAssertions;
using MainSolutionTemplate.Utilities.Cache;
using NUnit.Framework;

namespace MainSolutionTemplate.Utilities.Tests.Cache
{
    [TestFixture]
    public class SimpleObjectCacheTests
    {
        private SimpleObjectCache _simpleObjectCache;

        #region Setup/Teardown

        public void Setup()
        {
            _simpleObjectCache = new SimpleObjectCache(TimeSpan.FromSeconds(1));
        }

        [TearDown]
        public void TearDown()
        {
        }

        #endregion

        [Test]
        public void Get_WhenCacheDoesExistShoudOnlyDoItOnce_ShouldNotGetValue()
        {
            // arrange
            Setup();
            _simpleObjectCache.Get("value", () => { return "newValue"; });
            // action
            string result = _simpleObjectCache.Get("value", () => { return "nootNe"; });
            // assert
            result.Should().Be("newValue");
        }

        [Test]
        public void Get_WhenCacheDoesExist_ShouldNotGetValue()
        {
            // arrange
            Setup();
            _simpleObjectCache.Set("value", "newValue");
            // action
            string result = _simpleObjectCache.Get("value", () => { return "nootNe"; });
            // assert
            result.Should().Be("newValue");
        }

        [Test]
        public void Get_WhenCacheDoesNotExist_ShouldReturnValue()
        {
            // arrange
            Setup();
            // action
            string result = _simpleObjectCache.Get("value", () => { return "newValue"; });
            // assert
            result.Should().Be("newValue");
        }

        [Test]
        public void Get_WhenWithNoSet_ShouldJustRetrieveTheValue()
        {
            // arrange
            Setup();
            _simpleObjectCache.Set("value", "newValue");
            // action
            var result = _simpleObjectCache.Get<string>("value");
            // assert
            result.Should().Be("newValue");
        }
    }
}