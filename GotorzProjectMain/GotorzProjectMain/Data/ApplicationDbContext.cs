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
    public DbSet<FlightRoute> FlightRoutes { get; set; }
    public DbSet<Flight> Flights { get; set; }
    public DbSet<Layover> Layovers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		// MAP TO TABLES
		// modelBuilder.Entity<User>().ToTable("Users");
		modelBuilder.Entity<Customer>().ToTable("Customers");
		modelBuilder.Entity<Employee>().ToTable("Employees");
		modelBuilder.Entity<VacationRequest>().ToTable("VacationRequests");
		modelBuilder.Entity<VacationOffer>().ToTable("VacationOffers");
		modelBuilder.Entity<FlightBooking>().ToTable("FlightBookings");
        modelBuilder.Entity<HotelBooking>().ToTable("HotelBookings");
		modelBuilder.Entity<ChatMessage>().ToTable("ChatMessages");
		modelBuilder.Entity<FlightRoute>().ToTable("FlightRoutes");
        modelBuilder.Entity<Flight>().ToTable("Flights");
        modelBuilder.Entity<Layover>().ToTable("Layovers");

        // PRIMARY KEYS
        modelBuilder.Entity<Customer>().HasKey("Id");
        modelBuilder.Entity<Employee>().HasKey("Id");
        modelBuilder.Entity<VacationRequest>().HasKey("VacationRequestId");
        modelBuilder.Entity<VacationOffer>().HasKey("VacationOfferId");
        modelBuilder.Entity<FlightBooking>().HasKey("FlightBookingId");
        modelBuilder.Entity<HotelBooking>().HasKey("HotelBookingId");
        modelBuilder.Entity<ChatMessage>().HasKey("MessageId");
        modelBuilder.Entity<FlightRoute>().HasKey("RouteId");
        modelBuilder.Entity<Flight>().HasKey("FlightId");
        modelBuilder.Entity<Layover>().HasKey("LayoverId");

        // FOREIGN KEY RELATIONSHIPS
        modelBuilder.Entity<Customer>()
            .HasOne(c => c.User)
            .WithOne()
            .HasForeignKey<Customer>(c => c.Id); // FK is also the PK

        modelBuilder.Entity<Employee>()
            .HasOne(c => c.User)
            .WithOne()
            .HasForeignKey<Employee>(c => c.Id); // FK is also the PK

        modelBuilder.Entity<VacationRequest>()
            .HasMany(r => r.Offers)
            .WithOne()
            .HasForeignKey(b => b.VacationOfferId);

        modelBuilder.Entity<VacationOffer>()
            .HasOne(o => o.HotelBooking)
            .WithOne()
            .HasForeignKey<HotelBooking>(b => b.VacationOfferId);

        modelBuilder.Entity<HotelBooking>()
            .HasIndex(b => b.VacationOfferId)
            .IsUnique();

        modelBuilder.Entity<VacationOffer>()
            .HasOne(o => o.FlightBooking)
            .WithOne()
            .HasForeignKey<FlightBooking>(b => b.VacationOfferId);

        modelBuilder.Entity<FlightBooking>()
            .HasIndex(b => b.VacationOfferId)
            .IsUnique();

        modelBuilder.Entity<FlightBooking>()
            .HasMany(b => b.FlightRoutes)
            .WithOne()
            .HasForeignKey(fb => fb.FlightBookingId);

        // Ved ikke om man skulle gøre collection navnet og type navnet ens?
        modelBuilder.Entity<FlightRoute>()
            .HasMany(e => e.Legs)
            .WithOne()
            .HasForeignKey(l => l.FlightRouteId);

        modelBuilder.Entity<FlightRoute>()
            .HasMany(e => e.Layovers)
            .WithOne()
            .HasForeignKey(l => l.FlightRouteId);

        // OTHER
        // By default, EF Core stores enums as integers. To store them as strings:
        modelBuilder.Entity<VacationRequest>()
			.Property(v => v.Status)
			.HasConversion<string>();

        modelBuilder.Entity<VacationOffer>()
            .Property(v => v.OfferStatus)
            .HasConversion<string>();

		// Specifies amount of digits in TotalPrice
		modelBuilder.Entity<FlightRoute>()
		.Property(r => r.TotalPrice)
		.HasPrecision(18, 2);

	}
}
