using System;
using MainSolutionTemplate.OAuth2.Interfaces;

namespace MainSolutionTemplate.OAuth2.DefaultValues
{
	public class DefaultRefreshToken : IRefreshToken
	{
		public string Id { get; set; }

		public string ClientId { get; set; }

		public string Subject { get; set; }

		public DateTime IssuedUtc { get; set; }

		public DateTime ExpiresUtc { get; set; }

		public string ProtectedTicket { get; set; }
	}
}