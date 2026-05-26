using Core.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {
            //Database.EnsureCreated();
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Trainer> Trainers { get; set; }

        public DbSet<Booking> Bookings { get; set; }

        public DbSet<Membership> Memberships { get; set; }
        public DbSet<MembershipType> MembershipTypes { get; set; }
        public DbSet<Workout> Workouts { get; set; }
        public DbSet<WorkoutType> WorkoutTypes { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var contactInfoConverter = new ValueConverter<ContactInfo, string>(
                v => v.Serialize(),
                v => ContactInfo.Deserialize(v) ?? CreateDefaultContactInfo()
            );

            modelBuilder.Entity<Client>(entity =>
            {
                entity.Property(c => c.Contact)
                    .HasConversion(contactInfoConverter);
            });

            modelBuilder.Entity<Trainer>(entity =>
            {
                entity.Property(t => t.Contact)
                    .HasConversion(contactInfoConverter);
            });

            modelBuilder.Entity<WorkoutType>(entity =>
                entity.HasIndex(wt => wt.Name).IsUnique()
            );

            modelBuilder.Entity<MembershipType>(entity =>
                entity.HasIndex(mt => mt.Name).IsUnique()
            );

        }

        private static ContactInfo CreateDefaultContactInfo()
        {
            throw new InvalidOperationException("Failed to deserialize ContactInfo");
        }
    }
}
