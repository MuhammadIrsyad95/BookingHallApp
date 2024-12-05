using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnePointBooking.Domain.Entities;
using System.Drawing;

namespace OnePointBooking.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomPackage> RoomPackages { get; set; }
        public DbSet<RoomSetup> RoomSetups { get; set; }
        public DbSet<Amenity> Amenities { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Service> Services { get; set; } 

        public DbSet<Booking> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Konfigurasi relasi antara Booking dan Room
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Room)
                .WithMany() // Atur sesuai kebutuhan, jika Room memiliki relasi banyak ke satu
                .HasForeignKey(b => b.RoomId)
                .OnDelete(DeleteBehavior.Restrict); // Nonaktifkan cascade delete

            // Konfigurasi relasi antara Booking dan RoomPackage
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.RoomPackage)
                .WithMany() // Atur sesuai kebutuhan
                .HasForeignKey(b => b.RoomPackageId)
                .OnDelete(DeleteBehavior.Restrict); // Nonaktifkan cascade delete

            // Konfigurasi relasi antara Booking dan RoomSetup
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.RoomSetup)
                .WithMany() // Atur sesuai kebutuhan
                .HasForeignKey(b => b.RoomSetupId)
                .OnDelete(DeleteBehavior.Restrict); // Nonaktifkan cascade delete
        }
    }

}
