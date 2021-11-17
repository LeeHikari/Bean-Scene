

namespace ReservationProject.Areas.Admin.Models.Report
{
    public class Index: Data.Sitting
    {
        public int Total { get=>Reservations.Count; }
        public int Pending { get => Reservations.Count; }
        public int Confirmed { get; set; }
        public int Seated { get; set; }
        public int Completed { get; set; }
        public int Cancelled { get; set; }
    }
}
