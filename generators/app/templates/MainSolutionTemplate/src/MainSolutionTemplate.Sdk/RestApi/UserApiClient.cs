using System.Threading.Tasks;
using MainSolutionTemplate.Sdk.RestApi.Base;
using MainSolutionTemplate.Shared;
using MainSolutionTemplate.Shared.Interfaces.Shared;
using MainSolutionTemplate.Shared.Models;
using MainSolutionTemplate.Shared.Models.Reference;
using MainSolutionTemplate.Utilities.Helpers;
using RestSharp;

namespace MainSolutionTemplate.Sdk.RestApi
{
    public class UserApiClient : BaseCrudApiClient<UserModel, UserDetailModel, UserReferenceModel>,
                                 IUserControllerActions, IUserControllerStandardLookups
    {
        public UserApiClient(RestConnectionFactory restConnectionFactory)
            : base(restConnectionFactory, RouteHelper.UserController)
        {
        }

        #region Implementation of IUserControllerActions

        public async Task<UserModel> Register(RegisterModel user)
        {
            RestRequest request = DefaultRequest(_apiPrefix.UriCombine(RouteHelper.UserControllerRegister), Method.POST);
            request.AddBody(user);
            return await ExecuteAndValidate<UserModel>(request);
        }

        public async Task<bool> ForgotPassword(string email)
        {
            RestRequest request = DefaultRequest(_apiPrefix.UriCombine(RouteHelper.UserControllerForgotPassword),
                                                 Method.GET);
            request.AddUrlSegment("email", email);
            return await ExecuteAndValidateBool(request);
        }

        #endregion
    }
}