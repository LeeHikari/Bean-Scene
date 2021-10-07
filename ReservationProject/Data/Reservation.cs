using System;

namespace ReservationProject.Data
{
    public class Reservation
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public int Duration { get; set; } //Minutes
        public DateTime EndTime { get => StartTime.AddMinutes(Duration); } //Pre-defining EndTime (Adding our duration into our StartTime)
        public int Guests { get; set; }
        public string Note { get; set; }


        //Relationships
        public int ReservationStatusId { get; set; }
        public ReservationStatus ReservationStatus { get; set; }

        public int ReservationSourceId { get; set; }
        public ReservationSource ReservationSource { get; set; }

        public int SittingId { get; set; }
        public Sitting Sitting { get; set; }

        public int PersonId { get; set; }
        public Person Person { get; set; }

    }
}