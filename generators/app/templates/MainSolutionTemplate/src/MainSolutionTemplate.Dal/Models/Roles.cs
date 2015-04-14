using System.Collections.Generic;
using MainSolutionTemplate.Dal.Models.Enums;

namespace MainSolutionTemplate.Dal.Models
{
	public static class Roles
	{
		static Roles()
		{
			// todo: Rolf Should load the roles from IOC is using EF
		}

        public static Role Admin
        {
            get
            {
                return new Role() { Name = "Admin" }
;
            }
        }

		public static Role Guest
		{
			get
			{
				return new Role() { Name = "Guest", Activities = new List<Activity>() { Activity.UserGet, Activity.UserSubscribe } };
			}
		}

	}
}