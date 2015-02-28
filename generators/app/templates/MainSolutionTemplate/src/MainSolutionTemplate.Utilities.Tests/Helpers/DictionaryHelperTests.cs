using System.Collections.Generic;
using FluentAssertions;
using MainSolutionTemplate.Utilities.Helpers;
using NUnit.Framework;

namespace MainSolutionTemplate.Utilities.Tests.Helpers
{
    [TestFixture]
    public class DictionaryHelperTests
    {
        [Test]
        public void GetOrAdd_GivenGivenDictionary_ShouldResult()
        {
            // arrange
            var dictionary = new Dictionary<string, string>();
            // action
            var addOrGet = DictionaryHelper.GetOrAdd(dictionary, "asdf", (value) => value + "value");
            // assert
            addOrGet.Should().StartWith("asdf");
        }
    }
}