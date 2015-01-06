using FluentAssertions;
using NUnit.Framework;

namespace MainSolutionTemplate.Dal.Tests
{
  [TestFixture]
  public class SampleTests
  {
    private Sample _sample;

    #region Setup/Teardown

    public void Setup()
    {
      _sample = new Sample();
    }

    #endregion

    [Test]
    public void Constructor_WhenCalled_ShouldNotBeNull()
    {
      // arrange
      Setup();
      // assert
      _sample.Should().NotBeNull();
    }

  }
}