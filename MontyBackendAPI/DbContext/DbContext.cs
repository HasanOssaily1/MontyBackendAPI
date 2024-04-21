using Microsoft.EntityFrameworkCore;
using MontyBackendAPI.Models;

public class MyContext : DbContext
{

    public MyContext(DbContextOptions<MyContext> options) : base(options) { }



    public DbSet<Users> Users { get; set; } = null!;
    public DbSet<Subscriptions> Subscriptions { get; set; } = null!;

    //protected override void OnModelCreating(ModelBuilder modelBuilder)
    //{
    //   modelBuilder.Entity<Subscriptions>()
    //  .HasOne(s => s.User)                   // Subscription has one User
    //  .WithMany(u => u.Subscriptions)        // User has many Subscriptions
    //  .HasForeignKey(s => s.UserId);        // Foreign key is UserId in Subscriptions

    //}
}

