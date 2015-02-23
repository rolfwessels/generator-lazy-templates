using System;
using System.Collections.Generic;
using System.Linq;
using MainSolutionTemplate.Api.AppStartup;
using MainSolutionTemplate.Api.Models;
using MainSolutionTemplate.Api.Models.Mappers;
using MainSolutionTemplate.Api.SignalR.Attributes;
using MainSolutionTemplate.Api.WebApi.Controllers;
using MainSolutionTemplate.Core.Managers.Interfaces;
using MainSolutionTemplate.Core.MessageUtil;
using MainSolutionTemplate.Core.MessageUtil.Models;
using MainSolutionTemplate.Dal.Models;
using MainSolutionTemplate.Dal.Models.Enums;

namespace MainSolutionTemplate.Api.SignalR
{
	[TokenAuthorize]
	public class UserHub : BaseHub , IUserHub, IUserHubEvents
	{
		private readonly UserController _userController;

		public UserHub(UserController userController, IConnectionStateMapping connectionStateMapping) : base(connectionStateMapping)
		{
			_userController = userController;
		}
		
		[HubAuthorizeActivity(Activity.UserGet)]
		public List<UserModel> Get()
		{
			return _userController.Get().ToList();
		}

		[HubAuthorizeActivity(Activity.UserGet)]
		public UserModel Get(Guid id)
		{
			return _userController.Get(id);
		}

		[HubAuthorizeActivity(Activity.UserPost)]
		public UserModel Post(UserDetailModel user)
		{
			var userModel = _userController.Post(user);
			return userModel;
		}

		[HubAuthorizeActivity(Activity.UserUpdate)]
		public UserModel Put(Guid id, UserDetailModel user)
		{
			return _userController.Put(id, user);
		}

		[HubAuthorizeActivity(Activity.UserDelete)]
		public bool Delete(Guid id)
		{
			return _userController.Delete(id);
		}


		[HubAuthorizeActivity(Activity.UserSubscribe)]
		public void OnUpdate(ValueUpdateModel<UserModel> user)
		{
			Clients.All.OnUpdate(user);
		}

		#region Overrides of BaseHub

		protected override void OnInitializeOnce()
		{
			Messenger.Default.Register<DalUpdateMessage<User>>(this, (r) =>
			{
				OnUpdate(r.ToValueUpdateModel<User, UserModel>());
			});
		}

		#endregion

		
	}

	
}