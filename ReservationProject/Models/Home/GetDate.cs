using System;
using System.ComponentModel.DataAnnotations;

namespace ReservationProject.Models.Home
{
    public class GetDate
    {

        //Local Properties for reservation
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date")]
        public DateTime ObtainDate { get; set; }

    }
}
