using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using AutoMapper;
using MainSolutionTemplate.Api.Models;
using MainSolutionTemplate.Api.Models.Mappers;
using MainSolutionTemplate.Core.Managers.Interfaces;

namespace MainSolutionTemplate.Api.WebApi.Controllers
{
	/// <summary>
	///     Api controller for managing all the tasks
	/// </summary>
	public class UserController : ApiController
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
		public IQueryable<UserModel> Get()
		{
			return _systemManager.GetUsers().ToUserModel();
		}

		/// <summary>
		///     Returns list of all the tasks
		/// </summary>
		/// <returns>
		/// </returns>
		[Route(RouteHelper.UserController)]
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
		public UserModel Put(Guid id, UserModel user)
		{
			var userFound = _systemManager.GetUser(id);
			if (userFound == null) throw new Exception(string.Format("Could not find user by id '{0}'", id));
			var saveUser = _systemManager.SaveUser(user.ToUser());
			return saveUser.ToUserModel();
		}

		/// <summary>
		///     Create an instance of an item
		/// </summary>
		/// <param name="user">The user.</param>
		/// <returns>
		/// </returns>
		[Route(RouteHelper.UserController)]
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
	}
}