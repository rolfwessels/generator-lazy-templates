using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using MainSolutionTemplate.Dal.Models;

namespace MainSolutionTemplate.Dal.Ef
{
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1405:ComVisibleTypeBaseTypesShouldBeComVisible")]
	public class GeneralDbContext : DbContext
	{
		public GeneralDbContext()
			: this("MainSolutionTemplateContext")
		{
		}

		public GeneralDbContext(string nameOrConnectionString) : base(nameOrConnectionString)
		{
		}

		public DbSet<User> Users { get; set; }
		public DbSet<Role> Roles { get; set; }
		public DbSet<Application> Applications { get; set; }
		public DbSet<Project> Projects { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<User>().Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
			modelBuilder.Entity<Role>().Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
			modelBuilder.Entity<Application>().Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
			modelBuilder.Entity<User>()
						.HasMany<Role>(s => s.Roles)
						.WithMany(c => c.Users);
            modelBuilder.Entity<Project>().Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
			base.OnModelCreating(modelBuilder);
		}
	}
}