using System.Collections.Generic;

namespace ReservationProject.Data
{
    public class Restaurant
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<Sitting> Sittings { get; set; } = new List<Sitting>();

        public List<Area> Areas { get; set; } = new List<Area>();
    }
}