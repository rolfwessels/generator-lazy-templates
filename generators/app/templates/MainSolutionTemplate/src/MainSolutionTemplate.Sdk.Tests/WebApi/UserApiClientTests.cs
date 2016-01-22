using System;
using System.Collections.Generic;
using System.Reflection;
using FizzWare.NBuilder;
using FluentAssertions;
using MainSolutionTemplate.Api.Tests.Helper;
using MainSolutionTemplate.Sdk.RestApi;
using MainSolutionTemplate.Sdk.Tests.Shared;
using MainSolutionTemplate.Shared.Models;
using MainSolutionTemplate.Shared.Models.Reference;
using NUnit.Framework;
using log4net;

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

        #region Overrides of CrudComponentTestsBase<UserModel,UserCreateUpdateModel>

        protected override IList<UserCreateUpdateModel> GetExampleData()
        {
            return Builder<UserCreateUpdateModel>.CreateListOfSize(2).All().WithValidModelData().Build();
        }

        #endregion

	}

    
}