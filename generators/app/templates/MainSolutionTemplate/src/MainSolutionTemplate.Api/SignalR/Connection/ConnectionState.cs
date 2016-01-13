using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web.Security;
using MainSolutionTemplate.Api.SignalR.Attributes;
using MainSolutionTemplate.Core.BusinessLogic.Components;
using MainSolutionTemplate.Dal.Models;
using MainSolutionTemplate.Dal.Models.Enums;

namespace MainSolutionTemplate.Api.SignalR.Connection
{
	public class ConnectionState
	{
		private readonly string _connectionId;
		private readonly ClaimsPrincipal _principal;
		private readonly User _user;


		public ConnectionState(string connectionId, ClaimsPrincipal principal, User user)
		{
			_connectionId = connectionId;
			_principal = principal;
			_user = user;
		}

		public string ConnectionId
		{
			get { return _connectionId; }
		}

		public ClaimsPrincipal Principal
		{
			get { return _principal; }
		}

		public User User
		{
			get { return _user; }
		}

		public string UserEmail
		{
			get { return _user == null ? "anyone" : _user.Email; }
		}

		public bool IsAuthorized(Activity[] activities)
		{
			if (Principal.IsAuthenticated())
			{
			    return RoleManager.IsAuthorizedActivity(activities, User.Roles.ToArray());
			}
			return false;
		}
	}
}