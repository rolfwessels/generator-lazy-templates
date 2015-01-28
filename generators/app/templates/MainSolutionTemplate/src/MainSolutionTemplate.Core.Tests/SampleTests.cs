using System.Reflection;
using FluentAssertions;
using NUnit.Framework;
using log4net;

namespace MainSolutionTemplate.Core.Tests
{
	[TestFixture]
	public class SampleTests
	{
		private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
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
			_log.Info("Run");
			// arrange
			Setup();
			// assert
			_sample.Should().NotBeNull();
		}
	}
}