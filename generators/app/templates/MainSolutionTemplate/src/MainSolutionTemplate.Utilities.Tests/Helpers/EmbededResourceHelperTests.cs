using System;
using FluentAssertions;
using MainSolutionTemplate.Utilities.Helpers;
using NUnit.Framework;

namespace MainSolutionTemplate.Utilities.Tests.Helpers
{
    [TestFixture]
    public class EmbededResourceHelperTests
    {
        [Test]
        public void ReadResource_GivenValidResource_ShouldReturnString()
        {
            // arrange
            const string path = "MainSolutionTemplate.Utilities.Tests.Resources.t.txt";
            // action
            var readResource = EmbededResourceHelper.ReadResource(path,typeof (EmbededResourceHelperTests).Assembly);
            // assert
            readResource.Should().Be("sample");
        }
        [Test]
        public void ReadResource_GivenInvalidResource_ShouldThrowException()
        {
            // arrange
            const string path = "MainSolutionTemplate.Utilities.Tests.Resources.not.txt";
            // action
            Action testCall = () =>
            {
                EmbededResourceHelper.ReadResource(path, typeof (EmbededResourceHelperTests).Assembly);
            };
            ;
            // assert

            testCall.ShouldThrow<ArgumentException>().WithMessage("MainSolutionTemplate.Utilities.Tests.Resources.not.txt resource does not exist in MainSolutionTemplate.Utilities.Tests assembly");
        }
    }
}