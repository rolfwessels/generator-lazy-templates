using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Runtime.Remoting.Contexts;
using FluentAssertions;
using MainSolutionTemplate.Api.Models;
using MainSolutionTemplate.Api.WebApi;
using MainSolutionTemplate.Core.Helpers;
using MainSolutionTemplate.Shared.Models;
using NUnit.Framework;
using RestSharp;
using log4net;

namespace MainSolutionTemplate.Api.Tests.Integration.WebApi
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


		[Test]
		public void Login_WhenCalled_ShouldHaveStatusCodeOk()
		{
			// arrange
			Setup();
			var request = new RestRequest(RouteHelper.UserControllerLogin, Method.POST);
			request.AddJsonBody(new LoginModel() {UserName = AdminUser, Password = AdminPassword});
			// action
			var restResponse = _client.Value.ExecuteWithLogging<UserModel>(request);
			// assert
			restResponse.StatusCode.Should().Be(HttpStatusCode.OK);
			restResponse.Data.Should().NotBeNull();
		}
		
		
		
	}
}