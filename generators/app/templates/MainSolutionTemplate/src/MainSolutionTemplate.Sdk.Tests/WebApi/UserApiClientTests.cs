using System.Reflection;
using FluentAssertions;
using MainSolutionTemplate.Sdk.OAuth;
using MainSolutionTemplate.Sdk.Tests.Shared;
using NUnit.Framework;
using log4net;

namespace MainSolutionTemplate.Sdk.Tests.WebApi
{
	[TestFixture]
	[Category("Integration")]
    public class UserApiClientTests : UserClientBaseTests
	{
		private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	    private UserApiClient _userApiClient;

	    public UserApiClientTests()
	    {
            
	    }

	    #region Setup/Teardown

	    protected override void Setup()
		{
            _userApiClient = new UserApiClient(_adminRequestFactory.Value);
            _log.Debug("Login as " + _adminUser.Value.UserName);
            SetRequiredData(_userApiClient, _userApiClient);
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
            var restResponse = _userApiClient.Get("$top=1&$orderby=Name desc&$filter=not startswith(tolower(Name),'new')").Result;
			// assert
            restResponse.Count.Should().Be(1);
		}  

		[Test]
		public void GetDatail_WhenCalledWithTopAndSomeFilter_ShouldDisplayOnlySelectedRecords()
		{
			// arrange
			Setup();
			// action
            var restResponse = _userApiClient.GetDetail("$top=1&$orderby=Name desc&$filter=not startswith(tolower(Name),'new')").Result;
			// assert
            restResponse.Count.Should().Be(1);
		}  

		[Test]
        public void GetPaged_WhenCalledWithInlineCountAllpages_ShouldDisplayOnlySelectedRecordsButWithACount()
		{
			// arrange
			Setup();
			// action
            var restResponse = _userApiClient.GetPaged("$top=1").Result;
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
            var restResponse = _userApiClient.GetDetailPaged("$top=1").Result;
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
            var restResponse = _userApiClient.GetDetail().Result;
            // assert
            restResponse.Count.Should().BeGreaterThan(0);
		}


	}

    
}