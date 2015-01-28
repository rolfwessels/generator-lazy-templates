using System;
using System.Threading.Tasks;
using MainSolutionTemplate.OAuth2.Interfaces;
using Microsoft.Owin.Security.Infrastructure;

namespace MainSolutionTemplate.OAuth2
{
	public class RefreshTokenProvider : IAuthenticationTokenProvider
	{
		private readonly IOAuthSecurity _authSecurity;
		private readonly IOAuthDataManager _ioAuthDataManager;

		public RefreshTokenProvider(IOAuthDataManager ioAuthDataManager, IOAuthSecurity authSecurity)
		{
			_ioAuthDataManager = ioAuthDataManager;
			_authSecurity = authSecurity;
		}

		#region IAuthenticationTokenProvider Members

		public async Task CreateAsync(AuthenticationTokenCreateContext context)
		{
			string clientid = context.Ticket.Properties.Dictionary["as:client_id"];

			if (string.IsNullOrEmpty(clientid))
			{
				return;
			}

			string refreshTokenId = Guid.NewGuid().ToString("n");
			var refreshTokenLifeTime = context.OwinContext.Get<string>("as:clientRefreshTokenLifeTime");

			var token = _ioAuthDataManager.CreateRefresherToken();
			token.Id = _authSecurity.GetHash(refreshTokenId);
			token.ClientId = clientid;
			token.Subject = context.Ticket.Identity.Name;
			token.IssuedUtc = DateTime.UtcNow;
			token.ExpiresUtc = DateTime.UtcNow.AddMinutes(Convert.ToDouble(refreshTokenLifeTime));

			context.Ticket.Properties.IssuedUtc = token.IssuedUtc;
			context.Ticket.Properties.ExpiresUtc = token.ExpiresUtc;

			token.ProtectedTicket = context.SerializeTicket();

			await _ioAuthDataManager.SaveRefreshToken(token);
			context.SetToken(refreshTokenId);
		}

		public async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
		{
			var hashedTokenId = _authSecurity.GetHash(context.Token);
			var refreshToken = await _ioAuthDataManager.GetRefreshToken(hashedTokenId);
			if (refreshToken != null)
			{
				context.DeserializeTicket(refreshToken.ProtectedTicket);
				await _ioAuthDataManager.DeleteRefreshToken(hashedTokenId);
			}
		}

		public void Create(AuthenticationTokenCreateContext context)
		{
			CreateAsync(context).Wait();
		}

		public void Receive(AuthenticationTokenReceiveContext context)
		{
			ReceiveAsync(context).Wait();
		}

		#endregion
	}
}