using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReservationProject.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            base.OnModelCreating(mb);

            mb.Entity<Restaurant>()
                .Property(r => r.Name).IsRequired().HasMaxLength(50);

            mb.Entity<Person>(
                eb =>
                {
                    eb.Property(r => r.FirstName).IsRequired().HasMaxLength(50);
                    eb.Property(r => r.LastName).IsRequired().HasMaxLength(50);
                    eb.Property(r => r.Email).IsRequired().HasMaxLength(150);
                    eb.Property(r => r.UserId).IsRequired().HasMaxLength(150);
                    eb.HasIndex(r => r.Email).IsUnique(true);

                });


            mb.Entity<ReservationSource>()
                .Property(r => r.Name).IsRequired().HasMaxLength(20);

            mb.Entity<ReservationStatus>()
                .Property(r => r.Name).IsRequired().HasMaxLength(20);

            mb.Entity<Reservation>(
                eb =>
                {
                    eb.Property(r => r.StartTime).IsRequired();
                    eb.Property(r => r.Duration).IsRequired();
                    eb.Property(r => r.Guests).IsRequired();
                    eb.Property(r => r.Note).HasMaxLength(100); ;
                });

            mb.Entity<Sitting>(
                eb =>
            {
                eb.Property(r => r.StartTime).IsRequired();
                eb.Property(r => r.EndTime).IsRequired();
                eb.Property(r => r.Name).IsRequired().HasMaxLength(50);
                eb.Property(r => r.Capacity).IsRequired();

            });

            mb.Entity<SittingType>()
                .Property(r => r.Name).IsRequired().HasMaxLength(50);

            mb.Entity<Table>()
                .Property(r => r.Name).IsRequired().HasMaxLength(50);

            mb.Entity<Area>()
                .Property(r => r.Name).IsRequired().HasMaxLength(50);

            new DomainDataSeeder(mb);
        }

        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<ReservationStatus> ReservationStatuses { get; set; }
        public DbSet<ReservationSource> ReservationSources { get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<Sitting> Sittings {get; set;}
        public DbSet<SittingType> Sittingtypes {get; set;}
        public DbSet<Table> Tables { get; set; }
    }
}
