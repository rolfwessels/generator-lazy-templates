using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using MainSolutionTemplate.OAuth2.Dal.Interfaces;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using log4net;

namespace MainSolutionTemplate.OAuth2
{
	public class AuthorizationServerProvider : OAuthAuthorizationServerProvider
	{
		private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private readonly IOAuthDataManager _oauthDataManager;
		private readonly IOAuthSecurity _oauthSecurity;


		public AuthorizationServerProvider(IOAuthDataManager oauthDataManager, IOAuthSecurity oauthSecurity)
		{
			_oauthDataManager = oauthDataManager;
			_oauthSecurity = oauthSecurity;
		}

		public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
		{
			try
			{
				string clientId;
				string clientSecret;

				if (!context.TryGetBasicCredentials(out clientId, out clientSecret))
				{
					context.TryGetFormCredentials(out clientId, out clientSecret);
				}

				if (context.ClientId == null)
				{
					context.Validated();
					context.SetError("invalid_clientId", "ClientId should be sent.");
					return;
				}

				var client = await _oauthDataManager.GetApplication(context.ClientId);

				if (client == null)
				{
					context.SetError("invalid_clientId",
					                 string.Format("Client '{0}' is not registered in the system.", context.ClientId));

					return;
				}

				if (!string.IsNullOrEmpty(client.Secret))
				{
					if (string.IsNullOrWhiteSpace(clientSecret))
					{
						context.SetError("invalid_clientId", "Client secret should be sent.");
						return;
					}

					if (client.Secret != _oauthSecurity.GetHash(clientSecret))
					{
						context.SetError("invalid_clientId", "Client secret is invalid.");
						return;
					}
				}

				if (!client.Active)
				{
					context.SetError("invalid_clientId", "Client is inactive.");
					return;
				}

				context.OwinContext.Set("as:clientAllowedOrigin", client.AllowedOrigin);
				context.OwinContext.Set("as:clientRefreshTokenLifeTime",
				                        client.RefreshTokenLifeTime.ToString(CultureInfo.InvariantCulture));
				context.Validated();
			}
			catch (Exception e)
			{
				_log.Error("AuthorizationServerProvider:log " + e.Message);

				throw;
			}
		}

		public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
		{
			IAuthorizedUser user = await _oauthDataManager.GetUserByUserIdAndPassword(context.UserName, context.Password);

			if (user == null)
			{
				context.SetError("invalid_grant", "The user name or password is incorrect.");
				return;
			}

			var identity = new ClaimsIdentity(context.Options.AuthenticationType);
			identity.AddClaim(new Claim(ClaimTypes.Name, user.UserId));

			string[] roles = await _oauthDataManager.GetRolesForUser(user);
			foreach (string role in roles)
			{
				identity.AddClaim(new Claim(ClaimTypes.Role, role));
			}

			var props = new AuthenticationProperties(new Dictionary<string, string>
				{
					{"client_id", context.ClientId ?? string.Empty},
					{"userName", user.UserId ?? string.Empty},
					{"displayName", user.DisplayName ?? string.Empty},
					{"permissions", string.Join(",", roles)}
				});

			var ticket = new AuthenticationTicket(identity, props);
			context.Validated(ticket);
			await _oauthDataManager.UpdateUserLastActivityDate(user);
		}

		public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
		{
			string originalClient = context.Ticket.Properties.Dictionary["as:client_id"];
			string currentClient = context.ClientId;

			if (originalClient != currentClient)
			{
				context.SetError("invalid_clientId", "Refresh token is issued to a different clientId.");

				return Task.FromResult<object>(null);
			}

			// Change auth ticket for refresh token requests
			var newIdentity = new ClaimsIdentity(context.Ticket.Identity);

			newIdentity.AddClaim(new Claim("newClaim", "newValue"));

			var newTicket = new AuthenticationTicket(newIdentity, context.Ticket.Properties);

			context.Validated(newTicket);

			return Task.FromResult<object>(null);
		}

		public override Task TokenEndpoint(OAuthTokenEndpointContext context)
		{
			foreach (var property in context.Properties.Dictionary)
			{
				context.AdditionalResponseParameters.Add(property.Key, property.Value);
			}
			return Task.FromResult<object>(null);
		}

		public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
		{
			var expectedRootUri = new Uri(context.Request.Uri, "/");
			if (expectedRootUri.AbsoluteUri == context.RedirectUri)
			{
				context.Validated();
			}
			return Task.FromResult<object>(null);
		}
	}
}