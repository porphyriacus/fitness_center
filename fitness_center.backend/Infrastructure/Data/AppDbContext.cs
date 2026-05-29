using Core.ValueObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
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
    public class AppDbContext : IdentityDbContext<IdentityUser> // DbContext
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
            base.OnModelCreating(modelBuilder);
            

            modelBuilder.Entity<WorkoutType>(entity =>
                entity.HasIndex(wt => wt.Name).IsUnique()
            );

            modelBuilder.Entity<MembershipType>(entity =>
                entity.HasIndex(mt => mt.Name).IsUnique()
            );

        }

    }
}
