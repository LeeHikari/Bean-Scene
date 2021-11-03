using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReservationProject.Areas.Admin.Models.Reservation
{
    public class Update : Models.Reservation.Create
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public SelectList ReservationStatuses { get; set; }
    }
}
