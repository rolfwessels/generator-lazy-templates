using MainSolutionTemplate.Dal.Models;

namespace MainSolutionTemplate.Dal.Ef.Migrations
{
	using System.Data.Entity.Migrations;

	public sealed class Configuration : DbMigrationsConfiguration<GeneralDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

		protected override void Seed(GeneralDbContext context)
		{
			context.UsersSet.AddOrUpdate(x => x.Email,
			                             new User()
				                             {
					                             Email = "admin@admin.com",
					                             Name = "Admin",
					                             HashedPassword = "password123"
				                             },
			                             new User()
				                             {
					                             Email = "guest@guest.com",
					                             Name = "Guest",
					                             HashedPassword = "password123"
				                             }
				);

		}
    }
}
