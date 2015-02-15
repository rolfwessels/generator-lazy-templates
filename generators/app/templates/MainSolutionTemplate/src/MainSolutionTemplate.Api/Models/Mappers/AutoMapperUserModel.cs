using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using MainSolutionTemplate.Dal.Models;

namespace MainSolutionTemplate.Api.Models.Mappers
{
	public static class AutoMapperUserModel
	{
		static AutoMapperUserModel()
		{
			Mapper.CreateMap<User, UserModel>();
			Mapper.CreateMap<UserModel, User>()
				  .ForMember(x => x.Id, opt => opt.Ignore())  
				  .ForMember(x => x.Roles, opt => opt.Ignore())
			      .ForMember(x => x.HashedPassword, opt => opt.Ignore())
			      .ForMember(x => x.CreateDate, opt => opt.Ignore())
			      .ForMember(x => x.UpdateDate, opt => opt.Ignore());

			Mapper.CreateMap<RegisterModel, User>()
				  .ForMember(x => x.Id, opt => opt.Ignore())  
				  .ForMember(x => x.Roles, opt => opt.Ignore())
				  .ForMember(x => x.LastLoginDate, opt => opt.Ignore())
			      .ForMember(x => x.HashedPassword, opt => opt.Ignore())
			      .ForMember(x => x.CreateDate, opt => opt.Ignore())
			      .ForMember(x => x.UpdateDate, opt => opt.Ignore());
			
		}

		public static User ToUser(this UserModel model, User user = null)
		{
			return Mapper.Map(model, user);
		}

		public static User ToUser(this RegisterModel model, User user = null)
		{
			return Mapper.Map(model, user);
		}

		public static IQueryable<UserModel> ToUserModel(this IQueryable<User> users)
		{
			return Mapper.Map<IQueryable<User>, List<UserModel>>(users).AsQueryable();
		}

		public static UserModel ToUserModel(this User user, UserModel model = null)
		{
			return Mapper.Map(user, model);
		}
	}
}