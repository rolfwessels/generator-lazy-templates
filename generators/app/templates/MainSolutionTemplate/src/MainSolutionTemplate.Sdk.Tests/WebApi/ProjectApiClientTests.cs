using System;
using System.Reflection;
using FizzWare.NBuilder;
using FluentAssertions;
using MainSolutionTemplate.Sdk.OAuth;
using MainSolutionTemplate.Sdk.Tests.Shared;
using MainSolutionTemplate.Shared.Models;
using NUnit.Framework;
using log4net;

namespace MainSolutionTemplate.Sdk.Tests.WebApi
{
	[TestFixture]
	[Category("Integration")]
    public class ProjectApiClientTests : ProjectClientBaseTests
	{
		private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	    private ProjectApiClient _projectApiClient;

	    #region Setup/Teardown

	    protected override void Setup()
		{
            _projectApiClient = new ProjectApiClient(_adminRequestFactory.Value);
            SetRequiredData(_projectApiClient, _projectApiClient);
		}

	    [TearDown]
		public void TearDown()
		{

		}

		#endregion
        
		[Test]
		public void Get_WhenCalledWithTopAndSomeFilter_ShouldDisplayOnlySelectedRecords()
		{
			// arrange
			Setup();
			// action
            var restResponse = _projectApiClient.Get("$top=1&$orderby=Name desc&$filter=not startswith(tolower(Name),'new')").Result;
			// assert
            restResponse.Count.Should().Be(1);
		}  

		[Test]
		public void GetDetail_WhenCalledWithTopAndSomeFilter_ShouldDisplayOnlySelectedRecords()
		{
			// arrange
			Setup();
			// action
            var restResponse = _projectApiClient.GetDetail("$top=1&$orderby=Name desc&$filter=not startswith(tolower(Name),'new')").Result;
			// assert
            restResponse.Count.Should().Be(1);
		}  

		[Test]
        public void GetPaged_WhenCalledWithInlineCountAllpages_ShouldDisplayOnlySelectedRecordsButWithACount()
		{
			// arrange
			Setup();
			// action
            var restResponse = _projectApiClient.GetPaged("$top=1").Result;
			// assert
		    restResponse.Should().NotBeNull();
		    restResponse.Items.Should().HaveCount(1);
            restResponse.Count.Should().BeGreaterThan(0);
		}
        
        [Test]
        public void GetDetailPaged_WhenCalledWithInlineCountAllpages_ShouldDisplayOnlySelectedRecordsButWithACount()
		{
			// arrange
			Setup();
			// action
            var restResponse = _projectApiClient.GetDetailPaged("$top=1").Result;
			// assert
		    restResponse.Should().NotBeNull();
		    restResponse.Items.Should().HaveCount(1);
            restResponse.Count.Should().BeGreaterThan(0);
		}

		[Test]
		public void Get_WhenCalled_ShouldHaveSomeContent()
		{
            // arrange
            Setup();
            // action
            var restResponse = _projectApiClient.GetDetail().Result;
            // assert
            restResponse.Count.Should().BeGreaterThan(0);
		}

    

	}

    
}