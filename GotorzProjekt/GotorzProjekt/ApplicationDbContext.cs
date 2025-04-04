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

			// Configure primary key for User
			modelBuilder.Entity<User>()
				.HasKey(u => u.UserId);

			// Configure Customer table
			modelBuilder.Entity<Customer>()
				.HasOne<User>()
				.WithOne()
				.HasForeignKey<Customer>(c => c.UserId);

			// Configure Employee table
			modelBuilder.Entity<Employee>()
				.HasOne<User>()
				.WithOne()
				.HasForeignKey<Employee>(e => e.UserId);
		}
	}
}
