using System;
using MainSolutionTemplate.OAuth2.DefaultValues;
using MainSolutionTemplate.OAuth2.Interfaces;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;

namespace MainSolutionTemplate.OAuth2
{
	public static class AuthorizationSetup
	{
		private static bool _isInitialized;
		private static readonly object _locker = new object();


		public static void Initialize(IAppBuilder appBuilder)
		{
			Initialize(appBuilder, new SampleOAuthDataManager(), new SHA256Security());
		}

		public static void Initialize(IAppBuilder appBuilder, IOAuthDataManager oauthDataManager)
		{
			Initialize(appBuilder, oauthDataManager, new SHA256Security());
		}

		public static void Initialize(IAppBuilder appBuilder, IOAuthDataManager oauthDataManager, IOAuthSecurity oauthSecurity)
		{
			Initialize(appBuilder, oauthDataManager, oauthSecurity, new AuthorizationSettings());
		}

		public static void Initialize(IAppBuilder appBuilder, IOAuthDataManager oauthDataManager, IOAuthSecurity oauthSecurity, AuthorizationSettings settings)
		{
			if (settings == null) throw new ArgumentNullException("settings");
			lock (_locker)
			{
				if (!_isInitialized)
				{
					_isInitialized = true;
					var oAuthOptions = new OAuthAuthorizationServerOptions
						{
							TokenEndpointPath = new PathString(settings.EndpointPath),
							Provider = new AuthorizationServerProvider(oauthDataManager, oauthSecurity),
							AllowInsecureHttp = true
						};
					if (settings.RefresherTokenEnabled)
					{
						oAuthOptions.RefreshTokenProvider = new RefreshTokenProvider(oauthDataManager, oauthSecurity);
						oAuthOptions.AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(settings.MaxRefresherTokenLifetimeInMinutes);
					}
					
					appBuilder.UseOAuthAuthorizationServer(oAuthOptions);
				}
			}
		}
	}
}