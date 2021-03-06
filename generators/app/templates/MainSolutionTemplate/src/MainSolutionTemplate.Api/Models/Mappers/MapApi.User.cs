﻿using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using MainSolutionTemplate.Dal.Models;
using MainSolutionTemplate.Dal.Models.Reference;
using MainSolutionTemplate.Shared.Models;
using MainSolutionTemplate.Shared.Models.Reference;

namespace MainSolutionTemplate.Api.Models.Mappers
{
    public static partial class MapApi
    {
        public static void Initialize()
        {
        }

        public static User ToDal(this UserCreateUpdateModel model, User user = null)
        {
            return Mapper.Map(model, user);
        }

        public static User ToDal(this RegisterModel model, User user = null)
        {
            return Mapper.Map(model, user);
        }

        public static UserModel ToModel(this User user, UserModel model = null)
        {
            return Mapper.Map(user, model);
        }

        public static IEnumerable<UserReferenceModel> ToReferenceModelList(IQueryable<User> users)
        {
            return Mapper.Map<IQueryable<User>, IEnumerable<UserReferenceModel>>(users);
        }

        public static IEnumerable<UserModel> ToModelList(IQueryable<User> users)
        {
            return Mapper.Map<IQueryable<User>, IEnumerable<UserModel>>(users);
        }

        public static IEnumerable<RoleModel> ToModels(this List<Role> users)
        {
            return Mapper.Map<List<Role>, IEnumerable<RoleModel>>(users);
        }

        #region Private Methods

        private static void MapUserModel()
        {
            Mapper.CreateMap<User, UserModel>();
            Mapper.CreateMap<Role, RoleModel>();
            Mapper.CreateMap<User, UserReferenceModel>();
            Mapper.CreateMap<UserReference, UserReferenceModel>();

            Mapper.CreateMap<UserCreateUpdateModel, User>()
                .ForMember(x => x.Email, opt => opt.MapFrom(x => x.Email.ToLower()))
                .ForMember(x => x.LastLoginDate, opt => opt.Ignore())
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(x => x.Roles, opt => opt.Ignore())
                .ForMember(x => x.HashedPassword, opt => opt.Ignore())
                .ForMember(x => x.DefaultProject, opt => opt.Ignore())
                .ForMember(x => x.CreateDate, opt => opt.Ignore())
                .ForMember(x => x.UpdateDate, opt => opt.Ignore());

            Mapper.CreateMap<RegisterModel, User>()
                .ForMember(x => x.Email, opt => opt.MapFrom(x => x.Email.ToLower()))
                .ForMember(x => x.LastLoginDate, opt => opt.Ignore())
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(x => x.Roles, opt => opt.Ignore())
                .ForMember(x => x.LastLoginDate, opt => opt.Ignore())
                .ForMember(x => x.HashedPassword, opt => opt.Ignore())
                .ForMember(x => x.DefaultProject, opt => opt.Ignore())
                .ForMember(x => x.CreateDate, opt => opt.Ignore())
                .ForMember(x => x.UpdateDate, opt => opt.Ignore());
        }

        #endregion
    }
}