using GotorzProjectMain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GotorzProjectMain.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
	{ }

	public DbSet<Employee> Employees { get; set; }
	public DbSet<Customer> Customers { get; set; }
	public DbSet<VacationRequest> VacationRequests { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		// Map each entity to its own table
		// modelBuilder.Entity<User>().ToTable("Users");
		modelBuilder.Entity<Customer>().ToTable("Customers");
		modelBuilder.Entity<Employee>().ToTable("Employees");
		modelBuilder.Entity<VacationRequest>().ToTable("VacationRequests");

		// By default, EF Core stores enums as integers. To store them as strings:
		modelBuilder.Entity<VacationRequest>()
			.Property(v => v.Status)
			.HasConversion<string>();
	}
}
