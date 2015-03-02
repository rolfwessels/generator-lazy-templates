using System.Linq;
using System.Security.Claims;
using MainSolutionTemplate.Api.SignalR.Attributes;
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
				if (User.Roles.Select(x => x.Name).Contains(Roles.Admin.Name)) return true;
				var userActivities = User.Roles.SelectMany(x => x.Activities);
				return activities.Select(userActivities.Contains).All(contains => contains);
			}
			return false;
		}
	}
}