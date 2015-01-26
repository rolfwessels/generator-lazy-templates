using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using MainSolutionTemplate.Dal.Models;

namespace MainSolutionTemplate.Dal.Ef
{
	public class GeneralDataContext : DbContext
	{
		public GeneralDataContext() : this("MainSolutionTemplateContext")
		{
		}

		public GeneralDataContext(string connnectionStringName) : base(connnectionStringName)
		{
		}

		public DbSet<User> Users { get; set; }
		public DbSet<Role> Roles { get; set; }

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