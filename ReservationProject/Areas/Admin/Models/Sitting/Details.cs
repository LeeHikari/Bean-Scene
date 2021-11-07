using System.ComponentModel.DataAnnotations;

namespace ReservationProject.Areas.Admin.Models.Sitting
{
    public class Details : Create
    {
        [Required]
        public int Id { get; set; }
    }
}
