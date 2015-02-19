using System.Net;
using System.Reflection;
using FluentAssertions;
using MainSolutionTemplate.Api.Tests.Integration.WebApi;
using MainSolutionTemplate.Api.WebApi;
using MainSolutionTemplate.Core.Helpers;
using NUnit.Framework;
using RestSharp;
using log4net;

namespace MainSolutionTemplate.Api.Tests.Integration
{
	[TestFixture]
	[Category("Integration")]
	public class StaticFilesTests : IntegrationTestsBase
	{
		private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		#region Setup/Teardown

		public void Setup()
		{

		}

		[TearDown]
		public void TearDown()
		{

		}

		#endregion

		[Test]
		public void Index_GivenIndexHtmlPath_ShouldLoadHtmlPage()
		{
			// arrange
			Setup();
			var request = AdminRequest("/", Method.GET);
			// action
			var restResponse = _client.Value.ExecuteWithLogging<StaticFilesTests>(request);
			// assert
			restResponse.Content.Should().Contain("<body");
			restResponse.StatusCode.Should().Be(HttpStatusCode.OK);
		}

		[Test]
		public void View_GivenLoginhtmlPath_ShouldLoadHtmlPage()
		{
			// arrange
			Setup();
			var request = AdminRequest("/views/login.html", Method.GET);
			// action
			var restResponse = _client.Value.ExecuteWithLogging<StaticFilesTests>(request);
			// assert
			restResponse.Content.Should().Contain("loginForm");
			restResponse.StatusCode.Should().Be(HttpStatusCode.OK);
		}

		[Test]
		public void Root_Givenjson_ShouldNotLoadTheData()
		{
			// arrange
			Setup();
			var request = AdminRequest("bower.json", Method.GET);
			// action
			var restResponse = _client.Value.ExecuteWithLogging<StaticFilesTests>(request);
			// assert
			restResponse.Content.Should().BeEmpty();
		}
		
		[Test]
		public void Root_GivenMissingFile_ShouldBeEmpty()
		{
			// arrange
			Setup();
			var request = AdminRequest("nowaythisfileisthere", Method.GET);
			// action
			var restResponse = _client.Value.ExecuteWithLogging<StaticFilesTests>(request);
			// assert
			restResponse.Content.Should().BeEmpty();
			// todo: Rolf We really want a 404 here at some stage

		}

	}
}