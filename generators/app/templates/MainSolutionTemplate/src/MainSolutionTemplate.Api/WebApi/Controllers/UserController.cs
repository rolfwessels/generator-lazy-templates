using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using MainSolutionTemplate.Api.Common;
using MainSolutionTemplate.Api.WebApi.Attributes;
using MainSolutionTemplate.Api.WebApi.ODataSupport;
using MainSolutionTemplate.Dal.Models.Enums;
using MainSolutionTemplate.Shared;
using MainSolutionTemplate.Shared.Interfaces.Shared;
using MainSolutionTemplate.Shared.Models;
using MainSolutionTemplate.Shared.Models.Reference;

namespace MainSolutionTemplate.Api.WebApi.Controllers
{
    /// <summary>
	///     Api controller for managing all the user
	/// </summary>
    [RoutePrefix(RouteHelper.UserController)]
	public class UserController : ApiController, IUserControllerActions, IUserControllerLookups
    {
	    private readonly UserCommonController _userCommonController;
	    
        public UserController(UserCommonController userCommonController)
        {
            _userCommonController = userCommonController;
        }

        /// <summary>
        ///     Returns list of all the users as references
        /// </summary>
        /// <returns>
        /// </returns>
        [Route,AuthorizeActivity(Activity.UserGet) , QueryToODataFilter]
        public Task<IEnumerable<UserReferenceModel>> Get()
        {   
            return _userCommonController.Get(Request.GetQuery());
        }

        /// <summary>
        /// Gets all users with their detail.
        /// </summary>
        /// <returns></returns>
        [Route(RouteHelper.WithDetail),AuthorizeActivity(Activity.UserGet), QueryToODataFilter]
        public Task<IEnumerable<UserModel>> GetDetail()
		{
		    return _userCommonController.GetDetail(Request.GetQuery());
		}


        /// <summary>
		///     Returns a user by his Id.
		/// </summary>
		/// <returns>
		/// </returns>
		[Route(RouteHelper.WithId),AuthorizeActivity(Activity.UserGet)]
		public Task<UserModel> Get(Guid id)
		{
            return _userCommonController.Get(id);
		}

	    /// <summary>
	    ///     Updates an instance of the user item.
	    /// </summary>
	    /// <param name="id">The identifier.</param>
	    /// <param name="user">The user.</param>
	    /// <returns>
	    /// </returns>
		[Route(RouteHelper.WithId),AuthorizeActivity(Activity.UserUpdate)]
		public Task<UserModel> Put(Guid id, UserDetailModel user)
		{
            return _userCommonController.Put(id, user);
		}

	    /// <summary>
	    ///     Add a new user
	    /// </summary>
	    /// <param name="user">The user.</param>
	    /// <returns>
	    /// </returns>
        [Route,AuthorizeActivity(Activity.UserPost)]
		public Task<UserModel> Post(UserDetailModel user)
		{
            return _userCommonController.Post(user);
		}

	    /// <summary>
	    ///     Deletes the specified user.
	    /// </summary>
	    /// <param name="id">The identifier.</param>
	    /// <returns>
	    /// </returns>
		[Route(RouteHelper.WithId),AuthorizeActivity(Activity.UserDelete)]
		public Task<bool> Delete(Guid id)
		{
            return _userCommonController.Delete(id);
		}

		#region Other actions

	    /// <summary>
	    /// Registers the specified user.
	    /// </summary>
	    /// <param name="user">The user.</param>
	    /// <returns></returns>
        [HttpPost, Route(RouteHelper.UserControllerRegister), AllowAnonymous]
		public Task<UserModel> Register(RegisterModel user)
		{
            return _userCommonController.Register(user);
		}


	    /// <summary>
	    /// Forgot the password sends user an email with his password.
	    /// </summary>
	    /// <param name="email">The email.</param>
	    /// <returns></returns>
		[HttpGet, Route(RouteHelper.UserControllerForgotPassword),AllowAnonymous]
		public Task<bool> ForgotPassword(string email)
		{
            return _userCommonController.ForgotPassword(email);
		}

		#endregion
	}
}