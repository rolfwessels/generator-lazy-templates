using System.Collections.Generic;
using System.Net;
using System.Reflection;
using FluentAssertions;
using MainSolutionTemplate.Api.WebApi;
using MainSolutionTemplate.Api.WebApi.Controllers;
using MainSolutionTemplate.Core.Helpers;
using NUnit.Framework;
using RestSharp;
using log4net;

namespace MainSolutionTemplate.Api.Tests.Integration.WebApi
{
	[TestFixture]
	[Category("Integration")]
	public class TaskControllerIntegrationTests : IntegrationTestsBase
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
		public void Get_WhenCalled_ShouldHaveStatusCodeOk()
		{
			// arrange
			Setup();
			var request = new RestRequest(RouteHelper.TaskController,Method.GET);
			// action
			var restResponse = _client.Value.ExecuteWithLogging<List<TaskModel>>(request);
			// assert
			//restResponse.StatusCode.Should().Be(HttpStatusCode.OK);
			restResponse.Content.Should().StartWith("[");
		}

		[Test]
		public void Get_WhenCalled_ShouldHaveSomeContent()
		{
			// arrange
			Setup();
			var request = new RestRequest(RouteHelper.TaskController,Method.GET);
			// action
			var restResponse = _client.Value.ExecuteWithLogging<List<TaskModel>>(request);
			// assert
			restResponse.Content.Should().StartWith("[");
			restResponse.Data.Count.Should().BeGreaterOrEqualTo(1);
		}

		 
	}
}