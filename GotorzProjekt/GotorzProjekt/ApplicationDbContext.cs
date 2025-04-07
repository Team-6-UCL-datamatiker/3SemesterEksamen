using GotorzProjekt.Models;
using Microsoft.EntityFrameworkCore;

namespace GotorzProjekt
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{ }

		public DbSet<User> Users { get; set; }
		public DbSet<Employee> Employees { get; set; }
		public DbSet<Customer> Customers { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			// Map each entity to its own table
			modelBuilder.Entity<User>().ToTable("Users");
			modelBuilder.Entity<Customer>().ToTable("Customers");
			modelBuilder.Entity<Employee>().ToTable("Employees");

			// Optionally, if you want to configure relationships or keys explicitly, you can do so:
			//modelBuilder.Entity<Customer>()
			//   .HasKey(c => c.UserId);
			//modelBuilder.Entity<Employee>()
			//   .HasKey(e => e.UserId);
		}
	}
}
