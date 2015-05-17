using System;
using System.Linq;
using System.Reflection;
using MainSolutionTemplate.Core.BusinessLogic.Components.Interfaces;
using MainSolutionTemplate.Core.MessageUtil;
using MainSolutionTemplate.Core.MessageUtil.Models;
using MainSolutionTemplate.Core.Vendor;
using MainSolutionTemplate.Dal.Models;
using MainSolutionTemplate.Dal.Models.Enums;
using MainSolutionTemplate.Dal.Models.Reference;
using MainSolutionTemplate.Dal.Persistance;
using MainSolutionTemplate.Dal.Validation;
using log4net;

namespace MainSolutionTemplate.Core.BusinessLogic.Components
{
    public class UserManager : IUserManager
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IGeneralUnitOfWork _generalUnitOfWork;
        private readonly IMessenger _messenger;
        private readonly IValidatorFactory _validationFactory;

        public UserManager(IGeneralUnitOfWork generalUnitOfWork, IMessenger messenger,
                           IValidatorFactory validationFactory)
        {
            _generalUnitOfWork = generalUnitOfWork;
            _messenger = messenger;
            _validationFactory = validationFactory;
        }

        #region IUserManager Members

        public IQueryable<User> GetUsers()
        {
            return _generalUnitOfWork.Users;
        }

        public IQueryable<UserReference> GetUsersAsReference()
        {
            return _generalUnitOfWork.Users.Select(x => new UserReference {Id = x.Id, Name = x.Name, Email = x.Email});
        }

        public User GetUser(Guid id)
        {
            return _generalUnitOfWork.Users.FirstOrDefault(x => x.Id == id);
        }

        public User SaveUser(User user, string password = null)
        {
            user.Email = user.Email.ToLower();
            User userFound = _generalUnitOfWork.Users.FirstOrDefault(x => x.Id == user.Id);
            user.HashedPassword = password != null || userFound == null
                                      ? PasswordHash.CreateHash(password ??
                                                                user.HashedPassword ?? DateTime.Now.ToString())
                                      : userFound.HashedPassword;
            _validationFactory.ValidateAndThrow(user);
            if (userFound == null)
            {
                _log.Info(string.Format("Adding user [{0}]", user));
                _generalUnitOfWork.Users.Add(user);
                _messenger.Send(new DalUpdateMessage<User>(user, UpdateTypes.Inserted));
                return user;
            }
            _log.Info(string.Format("Update user [{0}]", user));
            _generalUnitOfWork.Users.Update(user);
            _messenger.Send(new DalUpdateMessage<User>(user, UpdateTypes.Updated));
            return user;
        }

        public User DeleteUser(Guid id)
        {
            User user = _generalUnitOfWork.Users.FirstOrDefault(x => x.Id == id);
            if (user != null)
            {
                _log.Info(string.Format("Remove user [{0}]", user));
                _generalUnitOfWork.Users.Remove(user);
                _messenger.Send(new DalUpdateMessage<User>(user, UpdateTypes.Removed));
            }
            return user;
        }

        public User GetUserByEmailAndPassword(string email, string password)
        {
            User user = GetUserByEmail(email);
            if (user != null && user.HashedPassword != null)
            {
                if (!PasswordHash.ValidatePassword(password, user.HashedPassword))
                {
                    user = null;
                    _log.Debug(string.Format("SystemManager:GetUserByEmailAndPassword {0}", password));
                    _log.Info(string.Format("Invalid password for user '{0}'", email));
                }
            }
            else
            {
                _log.Info(string.Format("Invalid user '{0}'", email));
            }
            return user;
        }

        public User GetUserByEmail(string email)
        {
            return _generalUnitOfWork.Users.FirstOrDefault(x => x.Email == email.ToLower());
        }

        public void UpdateLastLoginDate(string email)
        {
            User userFound = GetUserByEmail(email);
            if (userFound == null) throw new ArgumentException("Invalid email address.");
            userFound.LastLoginDate = DateTime.Now;
            _generalUnitOfWork.Users.Update(userFound);
        }

        #endregion
    }
}