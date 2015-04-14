using AutoMapper;
using MainSolutionTemplate.Dal.Models;
using MainSolutionTemplate.OAuth2.Dal.Interfaces;

namespace MainSolutionTemplate.Core.Mappers
{
	public static class MapCore
	{
		static MapCore()
		{
			Mapper.CreateMap<Application, OAuthClientAdapter>();
            Mapper.CreateMap<User, OAuthClientAddaptor>()
                  .ForMember(x => x.UserId, opt => opt.MapFrom(x => x.Email))
                  .ForMember(x => x.DisplayName, opt => opt.MapFrom(x => x.Name));
            
		}

		private class OAuthClientAdapter : IOAuthClient
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

        public static IAuthorizedUser MapToIAuthorizedUser(this User value)
        {
            return Mapper.Map<User, OAuthClientAddaptor>(value);
        }

        #region Nested type: OAuthClientAddaptor

        public class OAuthClientAddaptor : IAuthorizedUser
        {
            #region Implementation of IAuthorizedUser

            public string UserId { get; set; }
            public string DisplayName { get; set; }

            #endregion
        }

        #endregion
	}
}