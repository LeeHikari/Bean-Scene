using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReservationProject.Data
{
    class DomainDataSeeder
    {
        public DomainDataSeeder(ModelBuilder mb)
        {
            mb.Entity<Restaurant>()
                .HasData(new Restaurant {Id = 1, Name = "Bean Scene" });

            mb.Entity<Area>()
                .HasData(
                    new Area { Id = 1, RestaurantId = 1, Name = "Main" },
                    new Area { Id = 2, RestaurantId = 1, Name = "Outside" },
                    new Area { Id = 3, RestaurantId = 1, Name = "Balcony" }
                );

            mb.Entity<Table>()
                .HasData(
                    new Table { Id = 1, AreaId = 1, Name = "M1" },
                    new Table { Id = 2, AreaId = 1, Name = "M2" },
                    new Table { Id = 3, AreaId = 1, Name = "M3" },
                    new Table { Id = 4, AreaId = 1, Name = "M4" },
                    new Table { Id = 5, AreaId = 1, Name = "M5" },
                    new Table { Id = 6, AreaId = 1, Name = "M6" },
                    new Table { Id = 7, AreaId = 1, Name = "M7" },
                    new Table { Id = 8, AreaId = 1, Name = "M8" },
                    new Table { Id = 9, AreaId = 1, Name = "M9" },
                    new Table { Id = 10, AreaId = 1, Name = "M10" },

                    new Table { Id = 11, AreaId = 2, Name = "O1" },
                    new Table { Id = 12, AreaId = 2, Name = "O2" },
                    new Table { Id = 13, AreaId = 2, Name = "O3" },
                    new Table { Id = 14, AreaId = 2, Name = "O4" },
                    new Table { Id = 15, AreaId = 2, Name = "O5" },
                    new Table { Id = 16, AreaId = 2, Name = "O6" },
                    new Table { Id = 17, AreaId = 2, Name = "O7" },
                    new Table { Id = 18, AreaId = 2, Name = "O8" },
                    new Table { Id = 19, AreaId = 2, Name = "O9" },
                    new Table { Id = 20, AreaId = 2, Name = "O10" },

                    new Table { Id = 21, AreaId = 3, Name = "B1" },
                    new Table { Id = 22, AreaId = 3, Name = "B2" },
                    new Table { Id = 23, AreaId = 3, Name = "B3" },
                    new Table { Id = 24, AreaId = 3, Name = "B4" },
                    new Table { Id = 25, AreaId = 3, Name = "B5" },
                    new Table { Id = 26, AreaId = 3, Name = "B6" },
                    new Table { Id = 27, AreaId = 3, Name = "B7" },
                    new Table { Id = 28, AreaId = 3, Name = "B8" },
                    new Table { Id = 29, AreaId = 3, Name = "B9" },
                    new Table { Id = 30, AreaId = 3, Name = "B10" }
                );

            mb.Entity<ReservationSource>()
                .HasData(
                    new ReservationSource { Id = 1, Name = "Phone" },
                    new ReservationSource { Id = 2, Name = "Email" },
                    new ReservationSource { Id = 3, Name = "In_Person" },
                    new ReservationSource { Id = 4, Name = "Online" },
                    new ReservationSource { Id = 5, Name = "Mobile_App" }
                );

            mb.Entity<ReservationStatus>()
                .HasData(
                    new ReservationStatus { Id = 1, Name = "New" },
                    new ReservationStatus { Id = 2, Name = "Pending" },
                    new ReservationStatus { Id = 3, Name = "Confirmed" },
                    new ReservationStatus { Id = 4, Name = "Cancelled" },
                    new ReservationStatus { Id = 5, Name = "Seated" },
                    new ReservationStatus { Id = 6, Name = "Completed" }
                );

            mb.Entity<SittingType>()
                .HasData(
                    new ReservationStatus { Id = 1, Name = "Breakfast" },
                    new ReservationStatus { Id = 2, Name = "Lunch" },
                    new ReservationStatus { Id = 3, Name = "Dinner" },
                    new ReservationStatus { Id = 4, Name = "Other" }
                );


        }
    }
}
