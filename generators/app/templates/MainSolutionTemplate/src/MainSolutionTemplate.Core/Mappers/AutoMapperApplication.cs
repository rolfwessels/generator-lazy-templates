using AutoMapper;
using MainSolutionTemplate.Dal.Models;
using MainSolutionTemplate.OAuth2.Dal.Interfaces;

namespace MainSolutionTemplate.Core.Mappers
{
	public static class AutoMapperApplication
	{
		static AutoMapperApplication()
		{
			Mapper.CreateMap<Application, OAuthClientAdapter>();
			Mapper.AssertConfigurationIsValid();
		}

		private abstract class OAuthClientAdapter : IOAuthClient
		{
			#region Implementation of IOAuthClient

			public string Secret { get; set; }
			public bool Active { get; set; }
			public string AllowedOrigin { get; set; }
			public double RefreshTokenLifeTime { get; set; }

			#endregion
		}

		public static IOAuthClient MapToIOAuthClient(this Application application)
		{
			return Mapper.Map<Application, OAuthClientAdapter>(application);
		}
		
		
	}
}