using System;
using MainSolutionTemplate.OAuth2.Dal.DefaultValues;
using MainSolutionTemplate.OAuth2.Dal.Interfaces;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;

namespace MainSolutionTemplate.OAuth2
{
	public static class OathAuthorizationSetup
	{
		private static bool _isInitialized;
		private static readonly object _locker = new object();
		private static OAuthAuthorizationServerOptions _oAuthOptions;
		private static OAuthBearerAuthenticationOptions _oAuthBearerAuthenticationOptions;


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
			if (_isInitialized) throw new Exception("Auth setup already done.");
			lock (_locker)
			{
				if (!_isInitialized)
				{
					_isInitialized = true;
					_oAuthOptions = new OAuthAuthorizationServerOptions
						{
							TokenEndpointPath = new PathString(settings.EndpointPath),
							Provider = new AuthorizationServerProvider(oauthDataManager, oauthSecurity),
							AllowInsecureHttp = true,
							AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(settings.MaxRefresherTokenLifetimeInMinutes)
						};
					if (settings.RefresherTokenEnabled)
					{
						_oAuthOptions.RefreshTokenProvider = new RefreshTokenProvider(oauthDataManager, oauthSecurity);
						
						
					}
					_oAuthBearerAuthenticationOptions = new OAuthBearerAuthenticationOptions() { };

					appBuilder.UseOAuthAuthorizationServer(_oAuthOptions);
					appBuilder.UseOAuthBearerAuthentication(_oAuthBearerAuthenticationOptions);
					
					
					
				}
			}


		}

		public static OAuthAuthorizationServerOptions OAuthOptions
		{
			get { return _oAuthOptions; }
		}

		public static OAuthBearerAuthenticationOptions OAuthBearerAuthenticationOptions
		{
			get { return _oAuthBearerAuthenticationOptions; }
		}
	}
}