using System.Collections.Generic;
using System.Linq;

namespace ReservationProject.Areas.Admin.Models.Report
{
    public class Index : Data.Sitting
    {

        public int Total { get => Reservations.Count(); }
        public int Pending { get => Reservations.Count(r => r.ReservationStatusId == 1); }
        public int Confirmed { get => Reservations.Count(r => r.ReservationStatusId == 2); }
        public int Seated { get => Reservations.Count(r => r.ReservationStatusId == 4); }
        public int Completed { get => Reservations.Count(r => r.ReservationStatusId == 5); }
        public int Cancelled { get => Reservations.Count(r => r.ReservationStatusId == 3); }
    }
}
