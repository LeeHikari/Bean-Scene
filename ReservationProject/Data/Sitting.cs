using System;
using System.Collections.Generic;
using System.Linq;

namespace ReservationProject.Data
{
    public class Sitting
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Capacity { get; set; }

        public int RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; }

        public bool IsClosed { get; set; }

        public int NumberOfGuests { get => Reservations.Sum(r=>r.Guests); } //Gets total number of guests for a sitting

        public int Vacancies { get => Capacity - NumberOfGuests;} //How many spare spots are available.

        public List<Reservation> Reservations { get; set; }


    }
}