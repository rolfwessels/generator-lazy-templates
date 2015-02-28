using System;
using System.Reflection;
using FluentAssertions;
using log4net;
using MainSolutionTemplate.Sdk.OAuth;
using MainSolutionTemplate.Sdk.Tests.Shared;
using MainSolutionTemplate.Sdk.Tests.WebApi;
using MainSolutionTemplate.Shared.Models;
using NUnit.Framework;

namespace MainSolutionTemplate.Sdk.Tests.OAuth
{
	[TestFixture]
	[Category("Integration")]
	public class OAuthApiClientTests : IntegrationTestsBase
	{
		private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	    private OAuthApiClient _oAuthApiClient;
	    private readonly TokenRequestModel _tokenRequestModel;

	    public OAuthApiClientTests()
	    {
	        _tokenRequestModel = new TokenRequestModel() { UserName = AdminUser, client_id = ClientId, Password = AdminPassword };
	    }

	    #region Setup/Teardown

		public void Setup()
		{
            _oAuthApiClient = new OAuthApiClient(_defaultRequestFactory.Value);
		}

	    [TearDown]
		public void TearDown()
		{

		}

		#endregion

		[Test]
		public void Token_WhenCalledWithValidCredentials_ShouldReturnAccessToken()
		{
			// arrange
			Setup();
		    // action
		    var result = _oAuthApiClient.GenerateToken(_tokenRequestModel).Result;
			// assert
		    result.AccessToken.Should().NotBeNull();
		}

		[Test]
		public void Token_WhenCalledWithValidCredentials_ShouldReturnMetaData()
		{
			// arrange
			Setup();
           
		    // action
            var result = _oAuthApiClient.GenerateToken(_tokenRequestModel).Result;
			// assert
            result.TokenType.Should().Be("bearer");
            result.ExpiresIn.Should().Be(86399);
            result.ClientId.Should().Be("MainSolutionTemplate.Api");
            result.UserName.Should().Be("admin");
            result.DisplayName.Should().Be("Admin user");
            result.Permissions.Should().Be("Admin");
		}

		
		[Test]
		public void Token_WhenCalledWithInValidCredentials_ShouldReturnMetaData()
		{
			// arrange
			Setup();
		    var tokenRequestModel = new TokenRequestModel() { UserName = AdminUser, client_id = ClientId, Password = AdminPassword + AdminPassword };
			// action
            Action testCall = () => { _oAuthApiClient.GenerateToken(tokenRequestModel).Wait(); };
		    // assert
		    testCall.ShouldThrow<Exception>().WithMessage("The user name or password is incorrect.");
		}

	}
}