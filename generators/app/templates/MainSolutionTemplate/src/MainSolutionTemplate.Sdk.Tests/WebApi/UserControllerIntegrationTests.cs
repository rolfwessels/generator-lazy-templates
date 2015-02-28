using System.Collections.Generic;
using System.Net;
using System.Reflection;
using FluentAssertions;
using MainSolutionTemplate.Sdk.Helpers;
using MainSolutionTemplate.Shared;
using MainSolutionTemplate.Shared.Models;
using NUnit.Framework;
using RestSharp;
using log4net;

namespace MainSolutionTemplate.Sdk.Tests.WebApi
{
	[TestFixture]
	[Category("Integration")]
	public class UserControllerIntegrationTests : IntegrationTestsBase
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
			
			var request = AdminRequest(RouteHelper.UserController,Method.GET);
			// action
			var restResponse = _client.Value.ExecuteWithLogging<List<UserModel>>(request);
			// assert
			restResponse.StatusCode.Should().Be(HttpStatusCode.OK);
		}
		
		
		[Test]
		public void Get_WhenCalledWithTop_ShouldDisplayOnlySelectedRecords()
		{
			// arrange
			Setup();

			var request = AdminRequest(RouteHelper.UserController + "?$top=2", Method.GET);
			// action
			var restResponse = _client.Value.ExecuteWithLogging<List<UserModel>>(request);
			// assert
			//restResponse.StatusCode.Should().Be(HttpStatusCode.OK);
			restResponse.Data.Count.Should().BeGreaterThan(0);
			restResponse.Data.Count.Should().BeLessOrEqualTo(2);
		}

		[Test]
		public void Get_WhenCalled_ShouldHaveSomeContent()
		{
			// arrange
			Setup();
			var request = AdminRequest(RouteHelper.UserController, Method.GET);
			// action
			var restResponse = _client.Value.ExecuteWithLogging<List<UserModel>>(request);
			// assert
			restResponse.Data.Count.Should().BeGreaterOrEqualTo(1);
		}


		
		
	}
}