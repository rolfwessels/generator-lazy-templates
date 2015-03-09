using System;
using System.Linq;
using FizzWare.NBuilder;
using FizzWare.NBuilder.Generators;
using FluentAssertions;
using MainSolutionTemplate.Sdk.OAuth;
using MainSolutionTemplate.Shared.Interfaces.Shared;
using MainSolutionTemplate.Shared.Models;
using NUnit.Framework;
using MainSolutionTemplate.Utilities.Helpers;

namespace MainSolutionTemplate.Sdk.Tests.Shared
{
    public abstract class UserClientBaseTests : IntegrationTestsBase
    {
        private IUserControllerActions _userHubClient;
        private IUserControllerStandardLookups _userStandardLookups;

        protected abstract void Setup();

        protected void SetRequiredData(IUserControllerActions userControllerActions, IUserControllerStandardLookups userStandardLookups)
        {
            _userHubClient = userControllerActions;
            _userStandardLookups = userStandardLookups;
        }


        [Test]
        public void Get_WhenCalled_ShouldHaveStatusCodeOk()
        {
            // arrange
            Setup();
            // action
            var userModels = _userStandardLookups.Get().Result;
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
            var userId = _userStandardLookups.Get().Result.ToArray().Select(x => x.Id).First();
            
            // action
            var userModels = _userHubClient.Get(userId);
            // assert
            userModels.Should().NotBeNull();
        }

        [Test]
        public void Post_WhenCalledWithInvalidDate_ShouldThrowException()
        {
            // arrange
            Setup();
            var userDetailModel = Builder<UserDetailModel>.CreateNew().Build();
            // action
            try
            {
                _userHubClient.Post(userDetailModel).Wait();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            Action testCall = () => { _userHubClient.Post(userDetailModel).Wait(); };
            // assert
            testCall.ShouldThrow<Exception>().WithMessage("Validation failed: \r\n -- 'Email' is not a valid email address.");
        }

        [Test]
        public void PostPutDelete_WhenWhenGivenValidModel_ShouldLookupModels()
        {
            // arrange
            Setup();
            var userModel = Builder<UserModel>.CreateListOfSize(2).All().With(x=>x.Email = GetRandom.Email()).Build();
            
            // action
            var userModels = _userHubClient.Post(userModel[0]).Result;
            var savedUser = _userHubClient.Get(userModels.Id).Result;
            var userModelLoad = _userHubClient.Put(userModels.Id, userModel[1]).Result;
            var removed = _userHubClient.Delete(userModels.Id).Result;
            var removedSecond = _userHubClient.Delete(userModels.Id).Result;
            var removedUser = _userHubClient.Get(userModels.Id).Result;

            // assert
            savedUser.Should().NotBeNull();
            removedUser.Should().BeNull();
            userModel[0].Email.ToLower().Should().Be(userModels.Email);
            userModel[1].Email.ToLower().Should().Be(userModelLoad.Email);
            removed.Should().BeTrue();
            savedUser.Should().NotBeNull();
            removedSecond.Should().BeFalse();
            
        }
    }
}