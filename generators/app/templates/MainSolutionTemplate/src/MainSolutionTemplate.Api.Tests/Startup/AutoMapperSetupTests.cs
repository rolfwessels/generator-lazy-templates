using AutoMapper;
using MainSolutionTemplate.Api.AppStartup;
using NUnit.Framework;

namespace MainSolutionTemplate.Api.Tests.Startup
{
    [TestFixture]
    public class AutoMapperSetupTests
    {
        public AutoMapperSetupTests()
        {
            AutoMapperSetup.Initialize();
        }

        [Test]
        public void AutoMapperSetup_WhenConstructed_ShouldNotBeNull()
        {
            // assert
            Mapper.AssertConfigurationIsValid();
        }

    }

}