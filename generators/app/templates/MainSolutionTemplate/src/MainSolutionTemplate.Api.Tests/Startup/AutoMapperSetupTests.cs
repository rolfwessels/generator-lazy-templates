using AutoMapper;
using MainSolutionTemplate.Web.AppStartup;
using NUnit.Framework;

namespace MainSolutionTemplate.Web.Tests.Startup
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