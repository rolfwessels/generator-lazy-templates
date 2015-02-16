using System;
using System.Linq;
using System.Web.Http;
using MainSolutionTemplate.Api.Models;
using MainSolutionTemplate.Api.Models.Mappers;
using MainSolutionTemplate.Api.SignalR;
using MainSolutionTemplate.Core.Managers.Interfaces;
using MainSolutionTemplate.Dal.Models;

namespace MainSolutionTemplate.Api.WebApi.Controllers
{
	/// <summary>
	///     Api controller for managing all the tasks
	/// </summary>
	public class UserController : ApiController, IUserHub
	{
		private readonly ISystemManagerFacade _systemManager;
		

		public UserController(ISystemManagerFacade systemManager)
		{
			_systemManager = systemManager;
		
		}

		/// <summary>
		///     Returns list of all the tasks
		/// </summary>
		/// <returns>
		/// </returns>
		[Route(RouteHelper.UserController)]
		[Authorize(Roles = "Admin")]
		public IQueryable<UserModel> Get()
		{
			//var applyTo = options.ApplyTo(_systemManager.GetUsers()) as IQueryable<User>;
			return _systemManager.GetUsers().ToUserModel().AsQueryable();
		}

		
		/// <summary>
		///     Returns list of all the tasks
		/// </summary>
		/// <returns>
		/// </returns>
		[Route(RouteHelper.UserControllerId)]
		[Authorize]
		public UserModel Get(Guid id)
		{
			return _systemManager.GetUser(id).ToUserModel();
		}

		/// <summary>
		///     Updates an instance of the task item
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <param name="user">The user.</param>
		/// <returns>
		/// </returns>
		[Route(RouteHelper.UserController)]
		[Authorize(Roles = "Admin")]
		public UserModel Put(Guid id, UserModel user)
		{
			var userFound = _systemManager.GetUser(id);
			if (userFound == null) throw new Exception(string.Format("Could not find user by id '{0}'", id));
			var saveUser = _systemManager.SaveUser(user.ToUser(userFound));
			return saveUser.ToUserModel();
		}

		/// <summary>
		///     Create an instance of an item
		/// </summary>
		/// <param name="user">The user.</param>
		/// <returns>
		/// </returns>
		[Route(RouteHelper.UserController)]
		[Authorize(Roles = "Admin")]
		public UserModel Post(UserModel user)
		{
			var savedUser = _systemManager.SaveUser(user.ToUser());
			return savedUser.ToUserModel();
		}

		/// <summary>
		///     Deletes the specified task.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns>
		/// </returns>
		[Route(RouteHelper.UserController)]
		public bool Delete(Guid id)
		{
			var deleteUser = _systemManager.DeleteUser(id);
			return deleteUser != null;
		}

		#region Other actions

		[AllowAnonymous]
		[Route(RouteHelper.UserControllerRegister)]
		[HttpPost]
		public UserModel Register(RegisterModel user)
		{
			var user1 = user.ToUser();
			user1.Roles.Add(Roles.Guest);
			var savedUser = _systemManager.SaveUser(user1);
			return savedUser.ToUserModel();
		}
		
		[AllowAnonymous]
		[Route(RouteHelper.UserControllerLogin)]
		[HttpPost]
		public UserModel Login(LoginModel model)
		{
			var userByUserameAndPassword = _systemManager.GetUserByEmailAndPassword(model.UserName, model.Password);
			return userByUserameAndPassword.ToUserModel();
		}

		#endregion
	}
}