using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using FizzWare.NBuilder;
using FizzWare.NBuilder.Generators;
using FluentAssertions;
using MainSolutionTemplate.Api.Tests.Helper;
using MainSolutionTemplate.Dal.Tests.Helpers;
using MainSolutionTemplate.Utilities.Helpers;
using log4net;
using MainSolutionTemplate.Sdk.SignalrClient;
using MainSolutionTemplate.Sdk.Tests.Shared;
using MainSolutionTemplate.Shared.Models;
using MainSolutionTemplate.Shared.Models.Enums;
using Microsoft.AspNet.SignalR.Client;
using NUnit.Framework;

namespace MainSolutionTemplate.Sdk.Tests.SignalR
{
    [TestFixture]
	[Category("Integration")]
	public class UserHubClientTests : UserClientBaseTests
	{
		private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private HubConnection _hubConnection;
        private UserHubClient _client;

        #region Setup/Teardown

        protected override void Setup()
		{	
			
			_log.Info(string.Format("Connecting to {0}", SignalRUri));
            var queryString = new Dictionary<string, string>() { { AdminToken.TokenType, AdminToken.AccessToken } };
			_hubConnection = new HubConnection(SignalRUri, queryString);
			_client = new UserHubClient(_hubConnection);
            SetRequiredData(_client, _client);
			_hubConnection.Start().Wait();
			_log.Info(string.Format("Connection made {0}", SignalRUri));
		}

		[TearDown]
		public void TearDown()
		{

		}

		#endregion

        [Test]
        public void TestTheSubscribeSystem_GivenSubscribetests_Shouldresult()
        {
            // arrange
            Setup();
            var userDetailModel = Builder<UserDetailModel>.CreateNew().WithValidModelData().With(x => x.Name = GetRandom.FirstName()).Build();
            
            var valueUpdateModels = new List<ValueUpdateModel<UserModel>>();
            _client.OnUpdate(valueUpdateModels.Add);
            // action
            _client.SubscribeToUpdates();
            var post = _client.Post(userDetailModel).Result;
            _client.Put(post.Id, userDetailModel).Wait();
            _client.UnsubscribeFromUpdates();
            _client.Delete(post.Id);
            
            var updateModels = valueUpdateModels.Where(x => x.Value.Id == post.Id);
            // assert
            updateModels.WaitFor(x => x.Count() >= 2);
            updateModels.Should().HaveCount(2);
            updateModels.Select(x=>x.UpdateType).Should().Contain(UpdateTypeCodes.Inserted);
            updateModels.Select(x => x.UpdateType).Should().Contain(UpdateTypeCodes.Updated);
        }


	}
}