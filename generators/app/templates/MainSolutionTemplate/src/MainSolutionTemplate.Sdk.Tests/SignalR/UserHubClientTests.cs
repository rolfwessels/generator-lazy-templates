using System.Reflection;
using log4net;
using MainSolutionTemplate.Sdk.SignalrClient;
using MainSolutionTemplate.Sdk.Tests.Shared;
using Microsoft.AspNet.SignalR.Client;
using NUnit.Framework;

namespace MainSolutionTemplate.Sdk.Tests.SignalR
{
    [TestFixture]
	[Category("Integration")]
	public class UserHubClientTests : UserClientBaseTests
	{
		private HubConnection _hubConnection;
        private UserHubClient _client;

        #region Setup/Teardown

        protected override void Setup()
        {
            _hubConnection = CreateHubConnection();
            _client = new UserHubClient(_hubConnection);
            SetRequiredData(_client);
			_hubConnection.Start().Wait();
		}

        [TearDown]
		public void TearDown()
		{

		}

		#endregion

        [Test]
        public void TestTheSubscribeSystem_GivenAnInsertOrPost_ShouldTriggerEvents()
        {
            BaseSimpleSubscriptionTest();
        }


	}
}