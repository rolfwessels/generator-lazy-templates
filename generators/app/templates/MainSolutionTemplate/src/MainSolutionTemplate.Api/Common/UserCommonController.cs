﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using MainSolutionTemplate.Api.Models.Mappers;
using MainSolutionTemplate.Api.WebApi.ODataSupport;
using MainSolutionTemplate.Core.BusinessLogic.Facade.Interfaces;
using MainSolutionTemplate.Dal.Models;
using MainSolutionTemplate.Shared.Interfaces.Shared;
using MainSolutionTemplate.Shared.Models;
using MainSolutionTemplate.Shared.Models.Reference;
using log4net;

namespace MainSolutionTemplate.Api.Common
{
    
    public class UserCommonController : IUserControllerActions
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ISystemManager _systemManager;

        public UserCommonController(ISystemManager systemManager)
        {
            _systemManager = systemManager;
        }


        public Task<IEnumerable<UserReferenceModel>> Get(string query = null)
        {
            return Task.Run(() => new QueryToODataWrapper<User, UserReferenceModel>(_systemManager.GetUsers(), query, MapApi.ToReferenceModelList) as IEnumerable<UserReferenceModel>);
        }

        public Task<IEnumerable<UserModel>> GetDetail(string query = null)
        {
            return Task.Run(() => new QueryToODataWrapper<User, UserModel>(_systemManager.GetUsers(), query, MapApi.ToModelList) as IEnumerable<UserModel>);
        }

        public Task<UserModel> Get(Guid id)
        {
            return Task.Run(() => _systemManager.GetUser(id).ToModel());
        }

        public Task<UserModel> Put(Guid id, UserDetailModel user)
        {
            return Task.Run(() =>
            {
                var userFound = _systemManager.GetUser(id);
                if (userFound == null) throw new Exception(string.Format("Could not find model by id '{0}'", id));
                var saveUser = _systemManager.SaveUser(user.ToDal(userFound));
                return saveUser.ToModel();
            });
        }

       
        public Task<UserModel> Post(UserDetailModel user)
        {   
            return Task.Run(() =>
            {
                var savedUser = _systemManager.SaveUser(user.ToDal());
                return savedUser.ToModel();
            });
        }

        
        public Task<bool> Delete(Guid id)
        {
            return Task.Run(() =>
            {
                var deleteUser = _systemManager.DeleteUser(id);
                return deleteUser != null;
            });
        }

        #region Other actions

        public Task<UserModel> Register(RegisterModel model)
        {
            return Task.Run(() =>
            {
                var user = model.ToDal();
                user.Roles.Add(Roles.Guest);
                var savedUser = _systemManager.SaveUser(user);
                return savedUser.ToModel();
            });
        }

        public Task<bool> ForgotPassword(string email)
        {
            return Task.Run(() =>
            {
                _log.Warn(string.Format("User has called forgot password. We should send him and email to [{0}].", email));
                // todo: Forgot password
                return true;
            });
        }

        #endregion
    }

   
}