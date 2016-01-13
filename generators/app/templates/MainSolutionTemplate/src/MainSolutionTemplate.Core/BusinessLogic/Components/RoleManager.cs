using System.Collections.Generic;
using System.Linq;
using MainSolutionTemplate.Core.BusinessLogic.Components.Interfaces;
using MainSolutionTemplate.Dal.Models;
using MainSolutionTemplate.Dal.Models.Enums;
using MainSolutionTemplate.Utilities.Helpers;

namespace MainSolutionTemplate.Core.BusinessLogic.Components
{
    public class RoleManager : IRoleManager
    {
        private static readonly List<Role> _roles;
        public static Role Admin = new Role() { Name = "Admin", Activities = EnumHelper.ToArray<Activity>().ToList()};
        public static Role Guest = new Role()
        {
            Name = "Guest",
            Activities = EnumHelper.ToArray<Activity>().Where(x => x.ToString().StartsWith("Read") || x == Activity.Subscribe).ToList()
        };

        static RoleManager() 
        {
            _roles = new List<Role>
            {
                Admin,
                Guest
            };
        }

        #region IRoleManager Members

        public Role GetRoleByName(string name)
        {
            return GetRole(name);
        }

        private static Role GetRole(string name)
        {
            return _roles.FirstOrDefault(x => x.Name == name);
        }

        private static IEnumerable<Activity> Activities(IEnumerable<string> rolesByName)
        {
            return _roles.Where(x => rolesByName.Contains(x.Name)).SelectMany(x => x.Activities).ToArray();
        }

        #endregion

        public static bool IsAuthorizedActivity(Activity[] activities, params string[] roleName)
        {
            if (roleName.Contains(Admin.Name)) return true;
            var allActivities = Activities(roleName).ToArray();
            return activities.All(allActivities.Contains);
        }

        
    }
}