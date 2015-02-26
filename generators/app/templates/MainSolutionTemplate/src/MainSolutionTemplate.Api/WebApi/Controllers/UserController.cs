using System;
using System.Linq;
using System.Web.Http;
using MainSolutionTemplate.Api.Common;
using MainSolutionTemplate.Api.WebApi.Attributes;
using MainSolutionTemplate.Dal.Models.Enums;
using MainSolutionTemplate.Shared;
using MainSolutionTemplate.Shared.Interfaces.Signalr;
using MainSolutionTemplate.Shared.Models;
using MainSolutionTemplate.Shared.Models.Reference;

namespace MainSolutionTemplate.Api.WebApi.Controllers
{
	/// <summary>
	///     Api controller for managing all the tasks
	/// </summary>
	public class UserController : ApiController, IUserControllerActions
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
		[Route(RouteHelper.UserController)]
		[AuthorizeActivity(Activity.UserGet)]
		public IQueryable<UserReferenceModel> Get()
	    {
	        return _userCommonController.Get();
	    }

		[Route(RouteHelper.UserController)]
		[AuthorizeActivity(Activity.UserGet)]
		public IQueryable<UserModel> GetDetail()
	    {
	        return _userCommonController.GetDetail();
	    }

		
		/// <summary>
		///     Returns list of all the tasks
		/// </summary>
		/// <returns>
		/// </returns>
		[Route(RouteHelper.UserControllerId)]
		[AuthorizeActivity(Activity.UserGet)]
		public UserModel Get(Guid id)
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
		[Route(RouteHelper.UserController)]
		[AuthorizeActivity(Activity.UserUpdate)]
		public UserModel Put(Guid id, UserDetailModel user)
		{
            return _userCommonController.Put(id, user);
		}

		/// <summary>
		///     Create an instance of an item
		/// </summary>
		/// <param name="user">The user.</param>
		/// <returns>
		/// </returns>
		[Route(RouteHelper.UserController)]
		[AuthorizeActivity(Activity.UserPost)]
		public UserModel Post(UserDetailModel user)
		{
            return _userCommonController.Post(user);
		}

		/// <summary>
		///     Deletes the specified task.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns>
		/// </returns>
		[Route(RouteHelper.UserController)]
		[AuthorizeActivity(Activity.UserDelete)]
		public bool Delete(Guid id)
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
		public UserModel Register(RegisterModel user)
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
		public bool ForgotPassword(string email)
		{
            return _userCommonController.ForgotPassword(email);
		}

		#endregion
	}
}