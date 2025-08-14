
using Microsoft.EntityFrameworkCore;
using System;
using Equinox.Models.DomainModels;   // <-- needed for entity types

namespace Equinox.Models
{
    public class EquinoxContext : DbContext
    {
        public EquinoxContext(DbContextOptions<EquinoxContext> options) : base(options) { }

        public DbSet<EquinoxClass> EquinoxClasses { get; set; } = default!;
        public DbSet<ClassCategory> ClassCategories { get; set; } = default!;
        public DbSet<Club> Clubs { get; set; } = default!;
        public DbSet<User> Users { get; set; } = default!;
        public DbSet<Booking> Bookings { get; set; } = default!;

        // ✅ NEW: Memberships DbSet (place INSIDE the class, with other DbSets)
        public DbSet<Membership> Memberships { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ✅ Seeding Clubs
            modelBuilder.Entity<Club>().HasData(
                new Club { ClubId = 1, Name = "Chicago Loop", PhoneNumber = "312-000-0001" },
                new Club { ClubId = 2, Name = "West Chicago", PhoneNumber = "312-000-0002" },
                new Club { ClubId = 3, Name = "Lincoln Park", PhoneNumber = "312-000-0003" }
            );

            // ✅ Seeding Class Categories with Image (non-nullable)
            modelBuilder.Entity<ClassCategory>().HasData(
                new ClassCategory { ClassCategoryId = 1, Name = "Boxing", Image = "boxing.jpg" },
                new ClassCategory { ClassCategoryId = 2, Name = "Yoga", Image = "yoga.jpg" },
                new ClassCategory { ClassCategoryId = 3, Name = "HIIT", Image = "hiit.jpg" }
            );

            // ✅ Seeding Users (Coaches)
            modelBuilder.Entity<User>().HasData(
                new User {
                    UserId = 1,
                    Name = "Coach Mike",
                    PhoneNumber = "555-1111",
                    Email = "mike@equinox.com",
                    DOB = new DateTime(1985, 5, 1),
                    IsCoach = true
                },
                new User {
                    UserId = 2,
                    Name = "Coach Lisa",
                    PhoneNumber = "555-2222",
                    Email = "lisa@equinox.com",
                    DOB = new DateTime(1990, 3, 12),
                    IsCoach = true
                }
            );

            // ✅ Seeding Equinox Classes
            modelBuilder.Entity<EquinoxClass>().HasData(
                new EquinoxClass {
                    EquinoxClassId = 1,
                    Name = "Boxing 101",
                    ClassPicture = "boxing101.jpg",
                    ClassDay = "Monday",
                    Time = "8 AM – 9 AM",
                    ClassCategoryId = 1,
                    CoachId = 1,
                    ClubId = 1
                },
                new EquinoxClass {
                    EquinoxClassId = 2,
                    Name = "Hatha Yoga",
                    ClassPicture = "hatha.jpg",
                    ClassDay = "Wednesday",
                    Time = "6 PM – 7 PM",
                    ClassCategoryId = 2,
                    CoachId = 2,
                    ClubId = 2
                },
                new EquinoxClass {
                    EquinoxClassId = 3,
                    Name = "HIIT Junior",
                    ClassPicture = "hiitjunior.jpg",
                    ClassDay = "Friday",
                    Time = "5 PM – 6 PM",
                    ClassCategoryId = 3,
                    CoachId = 1,
                    ClubId = 3
                }
            );

            // 🔒 Phase 4: Prevent cascading deletes (match your actual nav & FK names)
            modelBuilder.Entity<EquinoxClass>()
                .HasOne(c => c.Club)
                .WithMany()
                .HasForeignKey(c => c.ClubId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<EquinoxClass>()
                .HasOne(c => c.ClassCategory)
                .WithMany()
                .HasForeignKey(c => c.ClassCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<EquinoxClass>()
                .HasOne(c => c.Coach)
                .WithMany()
                .HasForeignKey(c => c.CoachId)
                .OnDelete(DeleteBehavior.Restrict);

            // ✅ NEW (optional): Seed Memberships so the grid isn't empty
            modelBuilder.Entity<Membership>().HasData(
                new Membership { MembershipId = 1, Name = "Annual",     Price = 599.00m },
                new Membership { MembershipId = 2, Name = "Monthly",    Price = 59.00m  },
                new Membership { MembershipId = 3, Name = "Punch Card", Price = 120.00m }
            );
        }
    }
}
