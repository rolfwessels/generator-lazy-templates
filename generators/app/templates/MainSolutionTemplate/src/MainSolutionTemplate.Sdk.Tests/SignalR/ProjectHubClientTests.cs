using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using FizzWare.NBuilder;
using FizzWare.NBuilder.Generators;
using FluentAssertions;
using MainSolutionTemplate.Api.Tests.Helper;
using MainSolutionTemplate.Core.Tests.Helpers;
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
	public class ProjectHubClientTests : ProjectClientBaseTests
	{
		private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private HubConnection _hubConnection;
        private ProjectHubClient _client;

        #region Setup/Teardown

        protected override void Setup()
		{

            _hubConnection = CreateHubConnection();
			_client = new ProjectHubClient(_hubConnection);
            SetRequiredData(_client);
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
            BaseSimpleSubscriptionTest();
        }
	}
}