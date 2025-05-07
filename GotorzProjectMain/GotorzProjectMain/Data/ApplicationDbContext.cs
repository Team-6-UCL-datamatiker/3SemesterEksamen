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
	public DbSet<VacationOffer> VacationOffers { get; set; }
    public DbSet<FlightBooking> FlightBookings { get; set; }
    public DbSet<HotelBooking> HotelBookings { get; set; }
	public DbSet<ChatMessage> ChatMessages { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		// Map each entity to its own table
		// modelBuilder.Entity<User>().ToTable("Users");
		modelBuilder.Entity<Customer>().ToTable("Customers");
		modelBuilder.Entity<Employee>().ToTable("Employees");
		modelBuilder.Entity<VacationRequest>().ToTable("VacationRequests");
		modelBuilder.Entity<VacationOffer>().ToTable("VacationOffers");
		modelBuilder.Entity<FlightBooking>().ToTable("FlightBookings");
        modelBuilder.Entity<HotelBooking>().ToTable("HotelBookings");
		modelBuilder.Entity<ChatMessage>().ToTable("ChatMessages");

		// By default, EF Core stores enums as integers. To store them as strings:
		modelBuilder.Entity<VacationRequest>()
			.Property(v => v.Status)
			.HasConversion<string>();

        modelBuilder.Entity<VacationOffer>()
            .Property(v => v.OfferStatus)
            .HasConversion<string>();

    }
}
