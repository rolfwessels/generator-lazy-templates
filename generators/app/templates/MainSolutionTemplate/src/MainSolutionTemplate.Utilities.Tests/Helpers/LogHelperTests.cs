using FluentAssertions;
using MainSolutionTemplate.Utilities.Helpers;
using NUnit.Framework;

namespace MainSolutionTemplate.Utilities.Tests.Helpers
{
    [TestFixture]
    public class LogHelperTests
    {

        [Test]
        public void Dump_GivenString_ShouldReturnString()
        {
            // arrange
            var value = "value";
            // action
            var dump = value.Dump();
            // assert
            dump.Should().Be("\"value\"");
        }

        [Test]
        public void Dump_GivenStringAndName_ShouldOutputTheValueAndReturnOriginal()
        {
            // arrange
            var value = "value";
            // action
            var dump = value.Dump("Test");
            // assert
            dump.Should().Be(value);
        }



         
    }
}