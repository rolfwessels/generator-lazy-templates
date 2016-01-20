using System.Linq;
using System.Threading.Tasks;
using MainSolutionTemplate.Core.BusinessLogic.Components;
using MainSolutionTemplate.Core.Vendor;
using MainSolutionTemplate.Dal.Models;
using MongoDB.Driver;

namespace MainSolutionTemplate.Dal.Mongo.Migrations.Versions
{
    public class MigrateInitialize : IMigration
    {
		public async Task Update(IMongoDatabase db)
		{
			await AddApplications(db);
            await AddUsers(db);

            #if DEBUG
            await MockData(db);
            #endif

		}

        private async Task AddUsers(IMongoDatabase db)
		{
			var users = new MongoRepository<User>(db);

            var emailIndex = Builders<User>.IndexKeys.Ascending(x => x.Email);
            users.Collection.Indexes.CreateOneAsync(emailIndex).Wait();

			var admin = new User() {Name = "Admin user", Email = "admin", HashedPassword = PasswordHash.CreateHash("admin!")};
            admin.Roles.Add(RoleManager.Admin.Name);
            await users.Add(admin);

			var guest = new User() { Name = "Guest", Email = "guest@guest.com", HashedPassword = PasswordHash.CreateHash("guest!") };
			guest.Roles.Add(RoleManager.Guest.Name);
			await users.Add(guest);


		}

        private async Task AddApplications(IMongoDatabase db)
        {
            var apps = new MongoRepository<Application>(db);
            await apps.Add(new Application() { Active = true, AllowedOrigin = "*", ClientId = "MainSolutionTemplate.Api" });
            await apps.Add(new Application() { Active = true, AllowedOrigin = "*", ClientId = "MainSolutionTemplate.Console" });
            await apps.Add(new Application() { Active = true, AllowedOrigin = "*", ClientId = "MainSolutionTemplate.App" });
        }

        
        private async Task MockData(IMongoDatabase db)
        {
            var projects = new MongoRepository<Project>(db);
            var saves = Enumerable.Range(1, 10)
                .Select(x => new Project() {Name = "Project " + x})
                .Select(projects.Add)
                .ToArray();
            await Task.WhenAll(saves);
        }
    }
}