﻿using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using MainSolutionTemplate.Core.MessageUtil.Models;
using MainSolutionTemplate.Dal.Models;
using MainSolutionTemplate.Dal.Models.Reference;
using MainSolutionTemplate.Shared.Models;
using MainSolutionTemplate.Shared.Models.Enums;
using MainSolutionTemplate.Shared.Models.Reference;

namespace MainSolutionTemplate.Api.Models.Mappers
{
	public static class MapUserModel
	{
		static MapUserModel()
		{
			Mapper.CreateMap<User, UserModel>();
			Mapper.CreateMap<UserReference, UserReferenceModel>();

			Mapper.CreateMap<UserDetailModel, User>()
			      .ForMember(x => x.Email, opt => opt.MapFrom(x => x.Email.ToLower()))
			      .ForMember(x => x.LastLoginDate, opt => opt.Ignore())
			      .ForMember(x => x.Id, opt => opt.Ignore())
			      .ForMember(x => x.Roles, opt => opt.Ignore())
			      .ForMember(x => x.HashedPassword, opt => opt.Ignore())
			      .ForMember(x => x.CreateDate, opt => opt.Ignore())
			      .ForMember(x => x.UpdateDate, opt => opt.Ignore());

			Mapper.CreateMap<RegisterModel, User>()
			      .ForMember(x => x.Email, opt => opt.MapFrom(x => x.Email.ToLower()))
					.ForMember(x => x.LastLoginDate, opt => opt.Ignore())
			      .ForMember(x => x.Id, opt => opt.Ignore())
			      .ForMember(x => x.Roles, opt => opt.Ignore())
			      .ForMember(x => x.LastLoginDate, opt => opt.Ignore())
			      .ForMember(x => x.HashedPassword, opt => opt.Ignore())
			      .ForMember(x => x.CreateDate, opt => opt.Ignore())
			      .ForMember(x => x.UpdateDate, opt => opt.Ignore());
		}

		public static User ToUser(this UserDetailModel model, User user = null)
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

        public static IQueryable<UserReferenceModel> ToUserModel(this IQueryable<UserReference> users)
		{
            return Mapper.Map<IQueryable<UserReference>, List<UserReferenceModel>>(users).AsQueryable();
		}

		public static UserModel ToUserModel(this User user, UserModel model = null)
		{
			return Mapper.Map(user, model);
		}

		public static ValueUpdateModel<TModel> ToValueUpdateModel<T, TModel>(this DalUpdateMessage<T> updateMessage)
		{
			return new ValueUpdateModel<TModel>(Mapper.Map<T, TModel>(updateMessage.Value), (UpdateTypeCodes) updateMessage.UpdateType);
		}
	}
}