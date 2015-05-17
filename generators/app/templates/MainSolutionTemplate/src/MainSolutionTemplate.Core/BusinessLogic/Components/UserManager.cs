using System;
using System.Linq;
using System.Reflection;
using MainSolutionTemplate.Core.BusinessLogic.Components.Interfaces;
using MainSolutionTemplate.Core.Vendor;
using MainSolutionTemplate.Dal.Models;
using MainSolutionTemplate.Dal.Models.Reference;
using MainSolutionTemplate.Dal.Persistance;
using log4net;

namespace MainSolutionTemplate.Core.BusinessLogic.Components
{
    public class UserManager : BaseManager<User>, IUserManager
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public UserManager(BaseManagerArguments maseManager) : base(maseManager)
        {
        }

        #region Overrides of BaseManager<User>

        public override User Save(User project)
        {
            return Save(project, null);
        }

        protected override void DefaultModelNormalize(User user)
        {
            user.Email = user.Email.ToLower();
        }

        #endregion

        #region IUserManager Members
        
        public IQueryable<UserReference> GetUsersAsReference()
        {
            return _generalUnitOfWork.Users.Select(x => new UserReference {Id = x.Id, Name = x.Name, Email = x.Email});
        }
        
        public User Save(User user, string password)
        {
            var found = Get(user.Id);
            DefaultModelNormalize(user);
            user.HashedPassword = password != null || found == null
                                      ? PasswordHash.CreateHash(password ??
                                                                user.HashedPassword ?? DateTime.Now.ToString())
                                      : found.HashedPassword;
            _validationFactory.ValidateAndThrow(user);
            if (found == null)
            {
                return Insert(user);
            }
            Update(user);
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

        #region Overrides of BaseManager<User>

        protected override IRepository<User> Repository
        {
            get { return _generalUnitOfWork.Users; }
        }

        #endregion
    }
}