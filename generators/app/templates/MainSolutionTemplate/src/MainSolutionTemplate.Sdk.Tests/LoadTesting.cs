using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading;
using FluentAssertions;
using log4net;
using MainSolutionTemplate.Api.AppStartup;
using MainSolutionTemplate.Dal.Mongo;
using MainSolutionTemplate.Dal.Persistance;
using MainSolutionTemplate.Sdk.RestApi;
using MainSolutionTemplate.Sdk.Tests.Shared;
using MainSolutionTemplate.Shared.Models;
using MainSolutionTemplate.Utilities.Helpers;
using NUnit.Framework;

namespace MainSolutionTemplate.Sdk.Tests
{
    [TestFixture]
    public class LoadTestingTest : IntegrationTestsBase
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private ProjectApiClient _projectApiClient;


        [Test]
        public void GatherTiming_GivenMultipleRequests_ShouldBeFaster()
        {
            // arrange
            _projectApiClient = new ProjectApiClient(_adminRequestFactory.Value);
            if (!_projectApiClient.Get().Result.Any())
            {
                _projectApiClient.Insert(new ProjectDetailModel() {Name = "Sample"}).Wait();
            }
            // action
            RunConcurrent(500, () => _projectApiClient.Get().Wait());
            // assert
        }

        private void RunConcurrent(int concurrency, Action parameterizedThreadStart)
        {
            
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var threads = new List<Thread>();

            for (int i = 0; i < concurrency; i++)
            {
                var thread = new Thread((d)=>parameterizedThreadStart());
                threads.Add(thread);
            }
            threads.ForEach(x => x.Start());
            threads.ForEach(x => x.Join());
            stopwatch.Stop();
            _log.Info("stopwatch.ElapsedMilliseconds - " + stopwatch.ElapsedMilliseconds);
            _log.Warn(string.Format("Done {0} per second", Math.Round((decimal)concurrency / (stopwatch.ElapsedMilliseconds / 1000), 2)));
            var mongoGeneralUnitOfWork = IocApi.Instance.Resolve<IGeneralUnitOfWork>() as MongoGeneralUnitOfWork;
            _log.Warn("LoadTestingTest:RunConcurrent "+mongoGeneralUnitOfWork.Stats.Dump());
        }
    }

}