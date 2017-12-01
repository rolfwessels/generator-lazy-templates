using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FizzWare.NBuilder;
using FluentAssertions;
using MainSolutionTemplate.Sdk.RestApi;
using MainSolutionTemplate.Sdk.Tests.Shared;
using MainSolutionTemplate.Shared.Models;
using MainSolutionTemplate.Shared.Models.Reference;
using NUnit.Framework;
using log4net;
using MainSolutionTemplate.Core.Tests.Helpers;
using MainSolutionTemplate.Dal.Models;
using MainSolutionTemplate.Utilities.Helpers;

namespace MainSolutionTemplate.Sdk.Tests.WebApi
{
	[TestFixture]
	[Category("Integration")]
    public class UserApiClientTests : CrudComponentTestsBase<UserModel, UserCreateUpdateModel, UserReferenceModel>
	{
		private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	    private UserApiClient _userApiClient;

	    #region Setup/Teardown

	    protected override void Setup()
		{
            _userApiClient = new UserApiClient(_adminRequestFactory.Value);
            SetRequiredData(_userApiClient);
            _log.Info("_log");
		}

	    protected override IList<UserCreateUpdateModel> GetExampleData()
	    {
	        var detailModels = Builder<User>
	            .CreateListOfSize(2)
	            .All()
	            .WithValidData()
	            .Build();
	        return detailModels.JsonClone<List<UserCreateUpdateModel>>();
        }

	    [TearDown]
		public void TearDown()
		{

		}

		#endregion

	    [Test]
        public void WhoAmI_GivenUserData_ShouldReturn()
	    {
	        // arrange
	        Setup();
	        // action
	        var userModel = _userApiClient.WhoAmI().Result;
	        // assert
	        userModel.Should().NotBeNull();
	        userModel.Email.Should().Be("admin");
	    }

	    [Test]
        public void Roles_WhenCalled_ShouldReturnAllRoleInformation()
	    {
	        // arrange
	        Setup();
	        // action
            var userModel = _userApiClient.Roles().Result;
	        // assert
	        userModel.Count.Should().BeGreaterOrEqualTo(2);
	        userModel.Select(x => x.Name).Should().Contain("Admin");
	    }

        [Test]
        public void Post_WhenCalledWithInvalidDuplicateEmail_ShouldThrowException()
        {
            // arrange
            Setup();
            var userDetailModel = Builder<UserCreateUpdateModel>.CreateNew().Build();
            // action
            try
            {
                _crudController.Insert(userDetailModel).Wait();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            Action testCall = () => { _crudController.Insert(userDetailModel).Wait(); };
            // assert
            testCall.ShouldThrow<Exception>().WithMessage("'Email' is not a valid email address.");
        }
        

	}

    
}