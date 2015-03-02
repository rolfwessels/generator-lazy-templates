using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using MainSolutionTemplate.Api.Common;
using MainSolutionTemplate.Api.WebApi.Attributes;
using MainSolutionTemplate.Dal.Models.Enums;
using MainSolutionTemplate.Shared;
using MainSolutionTemplate.Shared.Interfaces.Shared;
using MainSolutionTemplate.Shared.Models;
using MainSolutionTemplate.Shared.Models.Reference;

namespace MainSolutionTemplate.Api.WebApi.Controllers
{
    

    /// <summary>
	///     Api controller for managing all the tasks
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
		///     Returns list of all the tasks
		/// </summary>
		/// <returns>
		/// </returns>
		[AuthorizeActivity(Activity.UserGet)]
        [Route]
		public Task<IQueryable<UserReferenceModel>> Get()
	    {
	        return _userCommonController.Get();
	    }

        
		
		[AuthorizeActivity(Activity.UserGet)]
        [Route(RouteHelper.WithDetail)]
		public Task<IQueryable<UserModel>> GetDetail()
	    {
	        return _userCommonController.GetDetail();
	    }

		
		/// <summary>
		///     Returns list of all the tasks
		/// </summary>
		/// <returns>
		/// </returns>
		[AuthorizeActivity(Activity.UserGet)]
		[Route(RouteHelper.WithId)]
		public Task<UserModel> Get(Guid id)
		{
            return _userCommonController.Get(id);
		}

	    /// <summary>
	    ///     Updates an instance of the task item
	    /// </summary>
	    /// <param name="id">The identifier.</param>
	    /// <param name="user">The user.</param>
	    /// <returns>
	    /// </returns>
		[AuthorizeActivity(Activity.UserUpdate)]
        [Route(RouteHelper.WithId)]
		public Task<UserModel> Put(Guid id, UserDetailModel user)
		{
            return _userCommonController.Put(id, user);
		}

	    /// <summary>
	    ///     Create an instance of an item
	    /// </summary>
	    /// <param name="user">The user.</param>
	    /// <returns>
	    /// </returns>
		[AuthorizeActivity(Activity.UserPost)]
        [Route]
		public Task<UserModel> Post(UserDetailModel user)
		{
            return _userCommonController.Post(user);
		}

	    /// <summary>
	    ///     Deletes the specified task.
	    /// </summary>
	    /// <param name="id">The identifier.</param>
	    /// <returns>
	    /// </returns>
		[AuthorizeActivity(Activity.UserDelete)]
        [Route(RouteHelper.WithId)]
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
	    [AllowAnonymous]
		[Route(RouteHelper.UserControllerRegister)]
		[HttpPost]
		public Task<UserModel> Register(RegisterModel user)
		{
            return _userCommonController.Register(user);
		}


	    /// <summary>
	    /// Forgot the password sends user an email with his password.
	    /// </summary>
	    /// <param name="email">The email.</param>
	    /// <returns></returns>
	    [AllowAnonymous]
		[Route(RouteHelper.UserControllerForgotPassword)]
		[HttpGet]
		public Task<bool> ForgotPassword(string email)
		{
            return _userCommonController.ForgotPassword(email);
		}

		#endregion
	}
}