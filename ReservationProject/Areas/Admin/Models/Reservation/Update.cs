using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReservationProject.Areas.Admin.Models.Reservation
{
    public class Update : Create
    {
        public Update(Data.Reservation reservation)
        {
            ResDate = reservation.StartTime.Date;
            ResTime = reservation.StartTime.ToLocalTime();
            Duration = reservation.Duration;
            Guests = reservation.Guests;
            Note = reservation.Note;
            ReservationStatusId = reservation.ReservationStatusId;
            ReservationSourceId = reservation.ReservationSourceId;
            SittingId = reservation.SittingId;
            FirstName = reservation.Person.FirstName;
            LastName = reservation.Person.LastName;
            Phone = reservation.Person.Phone;
            Email = reservation.Person.Email;
            RestaurantId = reservation.Sitting.RestaurantId;
            PersonId = reservation.PersonId;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public int PersonId { get; set; }
        public SelectList ReservationStatuses { get; set; }
    }
}
