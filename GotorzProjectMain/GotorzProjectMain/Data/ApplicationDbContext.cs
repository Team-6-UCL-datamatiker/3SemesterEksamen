using GotorzProjectMain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GotorzProjectMain.Data;

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
    }
}
