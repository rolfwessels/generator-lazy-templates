using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FizzWare.NBuilder;
using FizzWare.NBuilder.Generators;
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
	public class UserApiClientTests : IntegrationTestsBase
	{
		private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	    private UserApiClient _userApiClient;

	    public UserApiClientTests()
	    {
            
	    }

	    #region Setup/Teardown

		public void Setup()
		{
            _userApiClient = new UserApiClient(_adminRequestFactory.Value);
            _log.Debug("Login as " + _adminUser.Value.UserName);
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
		public void Get_WhenCalledWithInlineCountAllpages_ShouldDisplayOnlySelectedRecordsButWithACount()
		{
			// arrange
			Setup();
			// action
            var restResponse = _userApiClient.Get("$inlinecount=allpages&$top=1").Result;
			// assert
            restResponse.Count.Should().Be(1);
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


        [Test]
        public void Get_WhenCalled_ShouldHaveStatusCodeOk()
        {
            // arrange
            Setup();
            // action
            var userModels = _userApiClient.Get().Result.ToList();
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
            var userModels = _userApiClient.Get(Guid.NewGuid()).Result;
            // assert
            userModels.Should().BeNull();
        }

        [Test]
        public void Get_WhenCalledWithValidGuild_ShouldLookupUser()
        {
            // arrange
            Setup();

            var userId = _userApiClient.Get().Result.ToArray().Select(x => x.Id).First();
            _log.Info("userId:" + userId);

            // action
            var userModels = _userApiClient.Get(userId);
            // assert
            userModels.Should().NotBeNull();
        }

        [Test]
        public void PostPutDelete_WhenWhenGivenValidModel_ShouldLookupModels()
        {
            // arrange
            Setup();

            var count = _userApiClient.Get().Result.Count;
            var userModel = Builder<UserModel>.CreateListOfSize(2).All().With(x => x.Email = GetRandom.Email()).Build();
            // action
            var userModels = _userApiClient.Post(userModel[0]).Result;
            var userModelLoad = _userApiClient.Put(userModels.Id, userModel[1]).Result;
            var removed = _userApiClient.Delete(userModels.Id).Result;
            var removedSecond = _userApiClient.Delete(userModels.Id).Result;
            // assert
            userModel[0].Email.ToLower().Should().Be(userModels.Email);
            userModel[1].Email.ToLower().Should().Be(userModelLoad.Email);
            removed.Should().BeTrue();
            removedSecond.Should().BeFalse();
            count.Should().Be(_userApiClient.Get().Result.Count);
        }
	}

    
}