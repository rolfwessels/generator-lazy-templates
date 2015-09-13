using MainSolutionTemplate.Core.Vendor;
using MainSolutionTemplate.Dal.Models;
using MongoDB.Driver;

namespace MainSolutionTemplate.Dal.Mongo.Migrations
{
	public class MigrateInitialize
	{
		public MigrateInitialize(IMongoDatabase db)
		{
			AddApplications(db);
			AddUsers(db);
		}

        private static void AddUsers(IMongoDatabase db)
		{
			var users = new MongoRepository<User>(db);
			var admin = new User() {Name = "Admin user", Email = "admin", HashedPassword = PasswordHash.CreateHash("admin!")};
			admin.Roles.Add(Roles.Admin);
            users.Add(admin).Wait();

			var guest = new User() { Name = "Guest", Email = "guest@guest.com", HashedPassword = PasswordHash.CreateHash("guest!") };
			guest.Roles.Add(Roles.Guest);
			users.Add(guest).Wait();
		}

        private static void AddApplications(IMongoDatabase db)
        {
            var mongoRepository = new MongoRepository<Application>(db);
            mongoRepository.Add(new Application() { Active = true, AllowedOrigin = "*", ClientId = "MainSolutionTemplate.Api" }).Wait();
            mongoRepository.Add(new Application() { Active = true, AllowedOrigin = "*", ClientId = "MainSolutionTemplate.Console" }).Wait();
            mongoRepository.Add(new Application() { Active = true, AllowedOrigin = "*", ClientId = "MainSolutionTemplate.App" }).Wait();
        }
	}
}