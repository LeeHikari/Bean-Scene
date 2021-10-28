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

        [Required]
        [Display(Name = "Name of Sitting")]
        public string Name { get; set; }

        //Local Properties for reservation
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Sitting Date")]
        public DateTime SitDate { get; set; }


        //Properties to create
        [Required]
        [DataType(DataType.Time)]
        [Display(Name = "Start Time")]
        public DateTime SitStartTime { get; set; }

        public DateTime StartTime { get => SitDate.AddHours(SitStartTime.Hour).AddMinutes(SitStartTime.Minute); }
        
        
        [Required]
        [DataType(DataType.Time)]
        [Display(Name = "End Time")]
        public DateTime SitEndTime { get; set;}


        public DateTime EndTime { get => SitDate.AddHours(SitEndTime.Hour).AddMinutes(SitEndTime.Minute); }



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
