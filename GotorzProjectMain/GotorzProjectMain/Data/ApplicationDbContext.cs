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
    public DbSet<LoginAttempt> LoginAttempts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // PRIMARY KEYS
        modelBuilder.Entity<Customer>().HasKey(c => c.Id);
        modelBuilder.Entity<Customer>().Property(c => c.Id).ValueGeneratedOnAdd();

        modelBuilder.Entity<Employee>().HasKey(e => e.Id);
        modelBuilder.Entity<Employee>().Property(e => e.Id).ValueGeneratedOnAdd();

        modelBuilder.Entity<VacationRequest>().HasKey(vr => vr.VacationRequestId);
        modelBuilder.Entity<VacationRequest>().Property(vr => vr.VacationRequestId).ValueGeneratedOnAdd();

        modelBuilder.Entity<VacationOffer>().HasKey(vo => vo.VacationOfferId);
        modelBuilder.Entity<VacationOffer>().Property(vo => vo.VacationOfferId).ValueGeneratedOnAdd();

        modelBuilder.Entity<FlightBooking>().HasKey(fb => fb.FlightBookingId);
        modelBuilder.Entity<FlightBooking>().Property(fb => fb.FlightBookingId).ValueGeneratedOnAdd();

        modelBuilder.Entity<HotelBooking>().HasKey(hb => hb.HotelBookingId);
        modelBuilder.Entity<HotelBooking>().Property(hb => hb.HotelBookingId).ValueGeneratedOnAdd();

        modelBuilder.Entity<ChatMessage>().HasKey(cm => cm.MessageId);
        modelBuilder.Entity<ChatMessage>().Property(cm => cm.MessageId).ValueGeneratedOnAdd();

        modelBuilder.Entity<FlightRoute>().HasKey(fr => fr.RouteId);
        modelBuilder.Entity<FlightRoute>().Property(fr => fr.RouteId).ValueGeneratedOnAdd();

        modelBuilder.Entity<Flight>().HasKey(f => f.FlightId);
        modelBuilder.Entity<Flight>().Property(f => f.FlightId).ValueGeneratedOnAdd();

        modelBuilder.Entity<Layover>().HasKey(l => l.LayoverId);
        modelBuilder.Entity<Layover>().Property(l => l.LayoverId).ValueGeneratedOnAdd();


        // FOREIGN KEY RELATIONSHIPS
        modelBuilder.Entity<Customer>()
            .HasOne(c => c.User)
            .WithOne()
            .HasForeignKey<Customer>(c => c.Id) // FK is also the PK
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Employee>()
            .HasOne(c => c.User)
            .WithOne()
            .HasForeignKey<Employee>(c => c.Id) // FK is also the PK
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<VacationRequest>()
            .HasMany(r => r.Offers)
            .WithOne()
            .HasForeignKey(o => o.VacationRequestId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<VacationOffer>()
            .HasOne(o => o.HotelBooking)
            .WithOne()
            .HasForeignKey<HotelBooking>(b => b.VacationOfferId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<HotelBooking>()
            .HasIndex(b => b.VacationOfferId)
            .IsUnique();

        modelBuilder.Entity<VacationOffer>()
            .HasOne(o => o.FlightBooking)
            .WithOne()
            .HasForeignKey<FlightBooking>(b => b.VacationOfferId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<FlightBooking>()
            .HasIndex(b => b.VacationOfferId)
            .IsUnique();

        modelBuilder.Entity<FlightBooking>()
            .HasMany(b => b.FlightRoutes)
            .WithOne()
            .HasForeignKey(fb => fb.FlightBookingId)
            .OnDelete(DeleteBehavior.Cascade);

        // Ved ikke om man skulle g�re collection navnet og type navnet ens?
        modelBuilder.Entity<FlightRoute>()
            .HasMany(e => e.Legs)
            .WithOne()
            .HasForeignKey(l => l.FlightRouteId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<FlightRoute>()
            .HasMany(e => e.Layovers)
            .WithOne()
            .HasForeignKey(l => l.FlightRouteId)
            .OnDelete(DeleteBehavior.Cascade);

        // OTHER
        // By default, EF Core stores enums as integers. To store them as strings:
        modelBuilder.Entity<VacationRequest>()
            .Property(v => v.Status)
            .HasConversion<string>();

        modelBuilder.Entity<VacationOffer>()
            .Property(v => v.OfferStatus)
            .HasConversion<string>();

        // Specifies amount of digits in TotalPrice ------------------------- Hvorfor er det her vigtigt? Virker ikke med floats og vi g�r det ikke nogen andre steder med price???
        //modelBuilder.Entity<FlightRoute>()
        //.Property(r => r.TotalPrice)
        //.HasPrecision(18, 2);

    }
}
