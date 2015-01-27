using System.Collections;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq.Expressions;
using MainSolutionTemplate.Dal.Models;

namespace MainSolutionTemplate.Dal.Ef
{
	public class GeneralDbContext : DbContext
	{
		public GeneralDbContext(string nameOrConnectionString) : base(nameOrConnectionString)
		{
		}

		public DbSet<User> UsersSet { get; set; }
		public DbSet<Role> RoleSet { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<User>().Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
			modelBuilder.Entity<Role>().Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
			modelBuilder.Entity<User>()
						.HasMany<Role>(s => s.Roles)
						.WithMany(c => c.Users);
			base.OnModelCreating(modelBuilder);
		}
	}
}