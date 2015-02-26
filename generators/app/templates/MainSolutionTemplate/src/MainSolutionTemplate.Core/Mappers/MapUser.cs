using AutoMapper;
using MainSolutionTemplate.Dal.Models;
using MainSolutionTemplate.OAuth2.Dal.Interfaces;

namespace MainSolutionTemplate.Core.Mappers
{
	public static class MapUser
	{
		static MapUser()
		{
			Mapper.CreateMap<User, OAuthClientAddaptor>()
			      .ForMember(x => x.UserId, opt => opt.MapFrom(x => x.Email))
			      .ForMember(x => x.DisplayName, opt => opt.MapFrom(x => x.Name));
            
            
			
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