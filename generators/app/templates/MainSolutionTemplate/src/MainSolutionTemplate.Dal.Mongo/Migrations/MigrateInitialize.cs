using MainSolutionTemplate.Core.Vendor;
using MainSolutionTemplate.Dal.Models;
using MongoDB.Driver;

namespace MainSolutionTemplate.Dal.Mongo.Migrations
{
	public class MigrateInitialize
	{
		public MigrateInitialize(MongoDatabase db)
		{
			AddApplications(db);
			var users = new MongoRepository<User>(db);
			var entity = new User() {Name = "Admin user", Email = "admin", HashedPassword = PasswordHash.CreateHash("admin!")};
			entity.Roles.Add(Roles.Admin);
			users.Add(entity);
		}

		private static void AddApplications(MongoDatabase db)
		{
			new MongoRepository<Application>(db)
				{
					new Application() {Active = true, AllowedOrigin = "*", ClientId = "MainSolutionTemplate.Api"},
					new Application() {Active = true, AllowedOrigin = "*", ClientId = "MainSolutionTemplate.Console"},
					new Application() {Active = true, AllowedOrigin = "*", ClientId = "MainSolutionTemplate.App"}
				};
		}
	}
}