using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FizzWare.NBuilder;
using FizzWare.NBuilder.Generators;
using FluentAssertions;
using MainSolutionTemplate.Sdk.SignalrClient;
using MainSolutionTemplate.Sdk.Tests.Shared;
using MainSolutionTemplate.Shared.Models;
using Microsoft.AspNet.SignalR.Client;
using NUnit.Framework;
using RestSharp;
using log4net;

namespace MainSolutionTemplate.Sdk.Tests.WebApi
{
	[TestFixture]
	[Category("Integration")]
	public class UserHubClientTests : IntegrationTestsBase
	{
		private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private HubConnection _hubConnection;
		private UserHubClient _userHubClient;

		#region Setup/Teardown

		public void Setup()
		{	
			
			_log.Info(string.Format("Connecting to {0}", SignalRUri));
			var queryString = new Dictionary<string, string>() { { _adminUser.Value.TokenType, _adminUser.Value.AccessToken } };
			_hubConnection = new HubConnection(SignalRUri, queryString);
			_userHubClient = new UserHubClient(_hubConnection);
			_hubConnection.Start().Wait();
			_log.Info(string.Format("Connection made {0}", SignalRUri));
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
			// action
			var userModels = _userHubClient.Get().Result.ToList();
			// assert
			userModels.Should().NotBeNull();
			userModels.Count.Should().BeGreaterThan(0);
		}
		
		[Test]
		public void Get_WhenCalledWithGuild_ShouldLookupUser()
		{
			// arrange
			Setup();
			// action
			var userModels = _userHubClient.Get(Guid.NewGuid()).Result;
			// assert
			userModels.Should().BeNull();
		}
		
		[Test]
		public void Get_WhenCalledWithValidGuild_ShouldLookupUser()
		{
			// arrange
			Setup();
            var userId = _userHubClient.Get().Result.ToArray().Select(x => x.Id).First();
			_log.Info("userId:" + userId);
			
			// action
			var userModels = _userHubClient.Get(userId);
			// assert
			userModels.Should().NotBeNull();
		}
		
		[Test]
        public void PostPutDelete_WhenWhenGivenValidModel_ShouldLookupModels()
		{
			// arrange
			Setup();
            var count = _userHubClient.Get().Result.Count;
			var userModel = Builder<UserModel>.CreateListOfSize(2).All().With(x=>x.Email = GetRandom.Email()).Build();
			// action
			var userModels = _userHubClient.Post(userModel[0]).Result;
            var userModelLoad = _userHubClient.Put(userModels.Id, userModel[1]).Result;
            var removed = _userHubClient.Delete(userModels.Id).Result;
            var removedSecond = _userHubClient.Delete(userModels.Id).Result;
			// assert
			userModel[0].Email.ToLower().Should().Be(userModels.Email);
			userModel[1].Email.ToLower().Should().Be(userModelLoad.Email);
			removed.Should().BeTrue();
			removedSecond.Should().BeFalse();
            count.Should().Be(_userHubClient.Get().Result.Count);
		}
	}
}