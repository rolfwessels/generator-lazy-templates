using FluentAssertions;
using MainSolutionTemplate.Utilities.Helpers;
using NUnit.Framework;

namespace MainSolutionTemplate.Utilities.Tests.Helpers
{
    [TestFixture]
    public class StringHelperTests
    {
        [Test]
        public void UriCombine_GivenBaseUrlAndAdd_ShouldResultInValidUrl()
        {
            // arrange
            var baseUrl = "http://sample";
            // action
            var uriCombine = baseUrl.UriCombine("t","tt/","i.html");
            // assert
            uriCombine.Should().Be("http://sample/t/tt/i.html");
        }

        [Test]
        public void ToTitleCase_GivenValue_ShouldResultValidResult()
        {
            // arrange
            var baseUrl = "rolf sample";
            // action
            var uriCombine = baseUrl.ToTitleCase();
            // assert
            uriCombine.Should().Be("Rolf Sample");
        }

        [Test]
        public void ToInitialCase_GivenValue_ShouldResultValidResult()
        {
            // arrange
            var baseUrl = "rolf sample";
            // action
            var uriCombine = baseUrl.ToInitialCase();
            // assert
            uriCombine.Should().Be("Rolf sample");
        }

        [Test]
        public void UnderScoreAndCamelCaseToHumanReadable_GivenValue_ShouldResultValidResult()
        {
            // arrange
            var baseUrl = "rolf sample_asRample";
            // action
            var uriCombine = baseUrl.UnderScoreAndCamelCaseToHumanReadable();
            // assert
            uriCombine.Should().Be("rolf sample as Rample");
        }


        [Test]
        public void GetEmailAddresses_GivenValue_ShouldResultValidResult()
        {
            // arrange
            var baseUrl = "rolf@f.com,rolf@fd.com rolffd";
            // action
            var uriCombine = baseUrl.GetEmailAddresses();
            // assert
            uriCombine.Should()
                .Contain("rolf@f.com")
                .And.Contain("rolf@fd.com")
                .And.HaveCount(2);
        }

        [Test]
        public void StripHtml_GivenHtml_ShouldRemoveTags()
        {
            // arrange
            var baseUrl = "<b>Rolf<strong></string></b>";
            // action
            var uriCombine = baseUrl.StripHtml();
            // assert
            uriCombine.Should().Be("Rolf");
        }


        [Test]
        public void EnsureDoesNotStartWith_GivenStartingValue_ShouldRemoveValue()
        {
            // arrange
            var baseUrl = "/test";
            // action
            var uriCombine = baseUrl.EnsureDoesNotStartWith("/");
            // assert
            uriCombine.Should().Be("test");
        }

        [Test]
        public void EnsureEndsWith_GivenStartingValue_ShouldRemoveValue()
        {
            // arrange
            var baseUrl = "/test";
            // action
            var uriCombine = baseUrl.EnsureEndsWith("/");
            // assert
            uriCombine.Should().Be("/test/");
        }


    }
}