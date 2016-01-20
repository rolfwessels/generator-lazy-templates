using FluentAssertions;
using MainSolutionTemplate.Utilities.Helpers;
using NUnit.Framework;

namespace MainSolutionTemplate.Utilities.Tests.Helpers
{
    [TestFixture]
    public class Log4NetInitializeAttributeTests
    {
        [Test]
        public void Log4NetInitializeAttribute_GiventestingFor_Shouldresult()
        {
            // arrange
            // action
            var log4NetInitializeAttribute = new Log4NetInitializeAttribute("TEst");
            // assert
            log4NetInitializeAttribute.Should().NotBeNull();
            log4net.GlobalContext.Properties["machineName"].Should().NotBeNull();
            log4net.GlobalContext.Properties["assemblyName"].Should().NotBeNull();
        }
    }
}