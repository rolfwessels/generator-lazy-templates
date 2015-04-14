using System.Collections.Generic;
using MainSolutionTemplate.Dal.Models.Enums;

namespace MainSolutionTemplate.Dal.Models
{
	public static class Roles
	{

        public static Role Admin
        {
            get
            {// todo: Rolf Should load the roles from IOC is using EF
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