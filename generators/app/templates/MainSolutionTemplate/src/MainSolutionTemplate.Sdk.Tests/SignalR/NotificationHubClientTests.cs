using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using FizzWare.NBuilder;
using FluentAssertions;
using log4net;
using MainSolutionTemplate.Api.AppStartup;
using MainSolutionTemplate.Core.BusinessLogic.Components;
using MainSolutionTemplate.Core.BusinessLogic.Components.Interfaces;
using MainSolutionTemplate.Core.Tests.Helpers;
using MainSolutionTemplate.Dal.Models;
using MainSolutionTemplate.Dal.Models.Reference;
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
    public class NotificationHubClientTests : IntegrationTestsBase
	{
		private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private HubConnection _hubConnection;
        private NotificationHubClient _notificationHubClient;

        #region Setup/Teardown

        protected void Setup()
		{

            _hubConnection = CreateHubConnection();
			_notificationHubClient = new NotificationHubClient(_hubConnection);
			_hubConnection.Start().Wait();
			_log.Info(string.Format("Connection made {0}", SignalRUri));
		}

		[TearDown]
		public void TearDown()
		{

		}

		#endregion

        [Test]
        public void SubscriptToUpdateService_GivenConnectionDetails_ShouldRegisterToProjectUpdates()
        {
            // arrange
            Setup();
            var project = Builder<Project>.CreateNew().Build();
            var projectManager = IocApi.Instance.Resolve<IProjectManager>();
            var responses = new List<ValueUpdateModel<ProjectReference>>();
            // action
            _notificationHubClient.Subscribe<ProjectReference>("Project", responses.Add).Wait();
            projectManager.Save(project);
            projectManager.Delete(project.Id);
            // assert
            responses.WaitFor(x => x.Count >= 2, 5000);
            responses.Should().HaveCount(2);
            responses.First().Value.ShouldBeEquivalentTo(project);
            responses.Select(x => x.UpdateType).Should().Contain(UpdateTypeCodes.Inserted);
            responses.Select(x => x.UpdateType).Should().Contain(UpdateTypeCodes.Removed);
        }

        protected HubConnection CreateHubConnection()
        {
            var queryString = new Dictionary<string, string> { { AdminToken.TokenType, AdminToken.AccessToken } };
            return new HubConnection(SignalRUri, queryString);
        }

	}

    
}