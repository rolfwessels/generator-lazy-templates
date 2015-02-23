using System.Security.Claims;
using MainSolutionTemplate.Dal.Models;

namespace MainSolutionTemplate.Api.SignalR
{
	public class ConnectionState
	{
		private readonly string _connectionId;
		private readonly ClaimsPrincipal _principal;
		private readonly User _userByEmail;


		public ConnectionState(string connectionId, ClaimsPrincipal principal, User userByEmail)
		{
			_connectionId = connectionId;
			_principal = principal;
			_userByEmail = userByEmail;
		}

		public string ConnectionId
		{
			get { return _connectionId; }
		}

		public ClaimsPrincipal Principal
		{
			get { return _principal; }
		}

		public User UserByEmail
		{
			get { return _userByEmail; }
		}

		public string UserEmail
		{
			get { return _userByEmail == null ? "anyone" : _userByEmail.Email; }
		}
	}
}