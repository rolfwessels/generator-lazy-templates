﻿using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Security;
using MainSolutionTemplate.Api.Models.Mappers;
using MainSolutionTemplate.Core.BusinessLogic.Components;
using MainSolutionTemplate.Core.BusinessLogic.Components.Interfaces;
using MainSolutionTemplate.Dal.Models;
using MainSolutionTemplate.Shared.Interfaces.Shared;
using MainSolutionTemplate.Shared.Models;
using MainSolutionTemplate.Shared.Models.Reference;
using log4net;

namespace MainSolutionTemplate.Api.Common
{
    public class UserCommonController : BaseCommonController<User, UserModel, UserReferenceModel, UserDetailModel>,
                                        IUserControllerActions
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IUserManager _userManager;

        public UserCommonController(IUserManager userManager) : base(userManager)
        {
            _userManager = userManager;
        }

        #region IUserControllerActions Members

        public async Task<UserModel> Register(RegisterModel model)
        {
            User user = model.ToDal();
            user.Roles.Add(RoleManager.Guest.Name);
            User savedUser = await _userManager.Save(user, model.Password);
            return savedUser.ToModel();
        }

        public Task<bool> ForgotPassword(string email)
        {
            return Task.Run(() =>
                {

                    _log.Warn(string.Format("User has called forgot password. We should send him and email to [{0}].",
                                            email));
                    return true;
                    
                });
        }

        #endregion

        #region Overrides of BaseCommonController<User,UserModel,UserReferenceModel,UserDetailModel>

        protected override async Task<User> AddAdditionalMappings(UserDetailModel model, User dal)
        {
            var addAdditionalMappings = await base.AddAdditionalMappings(model, dal);
            if (!addAdditionalMappings.Roles.Any()) addAdditionalMappings.Roles.Add(RoleManager.Guest.Name);
            return addAdditionalMappings;
        }

        #endregion
    }
}