using System;

namespace MainSolutionTemplate.OAuth2.Dal.Interfaces
{
	public interface IRefreshToken
	{
		string Id { get; set; }

		string ClientId { get; set; }

		string Subject { get; set; }

		DateTime IssuedUtc { get; set; }

		DateTime ExpiresUtc { get; set; }

		string ProtectedTicket { get; set; }
	}
}