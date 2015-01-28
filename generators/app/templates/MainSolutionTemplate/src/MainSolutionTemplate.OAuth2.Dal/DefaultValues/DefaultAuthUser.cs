using System;
using MainSolutionTemplate.OAuth2.Dal.Interfaces;

namespace MainSolutionTemplate.OAuth2.Dal.DefaultValues
{
	public class DefaultAuthUser : IAuthorizedUser
	{
		private readonly string[] _roles;

		public DefaultAuthUser(string userId, string displayName, string password, string[] roles)
		{
			_roles = roles;
			UserId = userId;
			DisplayName = displayName;
			Password = password;
		}

		#region Implementation of IAuthorizedUser

		public string UserId { get; set; }
		public string DisplayName { get; set; }

		#endregion

		public string Password { get; set; }

		public string[] Roles
		{
			get { return _roles; }
		}

		public DateTime? LastLoggedIn { get; set; }
	}
}