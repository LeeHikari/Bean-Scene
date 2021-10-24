using Microsoft.AspNetCore.Mvc.Rendering;
using ReservationProject.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReservationProject.Areas.Admin.Models.Sitting
{
    public class Create
    {
        //Properties to create
        [Required]
        [Display(Name = "Name of Sitting")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Start Time")]
        public DateTime StartTime { get; set; }
        [Required]
        [Display(Name = "End Time")]
        public DateTime EndTime { get; set; }
        [Required]
        [Display(Name = "Capacity")]
        public int Capacity { get; set; }
        [Required]
        [Display(Name = "Restaurant")]
        public int RestaurantId { get; set; }

        public SelectList Restaurants { get; set; }

        public bool IsClosed { get; set; }



    }
}
