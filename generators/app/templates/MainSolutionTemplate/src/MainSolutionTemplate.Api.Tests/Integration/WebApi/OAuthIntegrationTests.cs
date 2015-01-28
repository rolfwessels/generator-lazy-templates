﻿using System.Collections.Generic;
using System.Reflection;
using FluentAssertions;
using MainSolutionTemplate.Api.WebApi.Controllers;
using MainSolutionTemplate.Core.Helpers;
using NUnit.Framework;
using RestSharp;
using log4net;

namespace MainSolutionTemplate.Api.Tests.Integration.WebApi
{
	[TestFixture]
	[Category("Integration")]
	public class OAuthIntegrationTests : IntegrationTestsBase
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
		public void Token_WhenCalledWithValidCredentials_ShouldReturnAccessToken()
		{
			// arrange
			Setup();
			var request = new RestRequest("Token", Method.POST);
			request.AddParameter("username", "admin");
			request.AddParameter("password", "admin!");
			request.AddParameter("grant_type", "password");
			request.AddParameter("client_id", "sampleApp");
			// action
			var restResponse = _client.Value.ExecuteWithLogging<List<TaskModel>>(request);
			// assert
			restResponse.Content.Should().Contain("access_token");
		}

		[Test]
		public void Token_WhenCalledWithValidCredentials_ShouldReturnMetaData()
		{
			// arrange
			Setup();
			var request = new RestRequest("Token", Method.POST);
			request.AddParameter("username", "admin");
			request.AddParameter("password", "admin!");
			request.AddParameter("grant_type", "password");
			request.AddParameter("client_id", "sampleApp");
			// action
			var restResponse = _client.Value.ExecuteWithLogging<List<TaskModel>>(request);
			// assert
			restResponse.Content.Should().Contain("permissions");
			restResponse.Content.Should().Contain("displayName");
			restResponse.Content.Should().Contain("userName");
			restResponse.Content.Should().Contain(".expires");
		}

		
		[Test]
		public void Token_WhenCalledWithInValidCredentials_ShouldReturnMetaData()
		{
			// arrange
			Setup();
			var request = new RestRequest("Token",Method.POST) ;
			request.AddParameter("username", "admin");
			request.AddParameter("password", "admin");
			request.AddParameter("grant_type", "password");
			request.AddParameter("client_id", "sampleApp");
			// action
			var restResponse = _client.Value.ExecuteWithLogging<List<TaskModel>>(request);
			// assert
			restResponse.Content.Should().Contain("The user name or password is incorrect");
		}

		

		 
	}
}