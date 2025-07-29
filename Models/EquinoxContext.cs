using Microsoft.EntityFrameworkCore;
using System;

namespace Equinox.Models
{
    public class EquinoxContext : DbContext
    {
        public EquinoxContext(DbContextOptions<EquinoxContext> options) : base(options) { }

        public DbSet<EquinoxClass> EquinoxClasses { get; set; }
        public DbSet<ClassCategory> ClassCategories { get; set; }
        public DbSet<Club> Clubs { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ✅ Seeding Clubs
            modelBuilder.Entity<Club>().HasData(
                new Club { ClubId = 1, Name = "Chicago Loop", PhoneNumber = "312-000-0001" },
                new Club { ClubId = 2, Name = "West Chicago", PhoneNumber = "312-000-0002" },
                new Club { ClubId = 3, Name = "Lincoln Park", PhoneNumber = "312-000-0003" }
            );

            // ✅ Seeding Class Categories
            modelBuilder.Entity<ClassCategory>().HasData(
                new ClassCategory { ClassCategoryId = 1, Name = "Boxing" },
                new ClassCategory { ClassCategoryId = 2, Name = "Yoga" },
                new ClassCategory { ClassCategoryId = 3, Name = "HIIT" }
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
        }
    }
}
