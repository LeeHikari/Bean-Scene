using Microsoft.AspNetCore.Mvc.Rendering;
using ReservationProject.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReservationProject.Areas.Admin.Models.Reservation
{
    public class Create
    {

        //Local Properties for reservation
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Reservation Date")]
        public DateTime ResDate { get; set; }

        [Required]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
        [Display(Name = "Reservation Time")]
        public DateTime ResTime { get; set; }

        //properties to create reservation
        public DateTime StartTime { get => ResDate.AddHours(ResTime.Hour).AddMinutes(ResTime.Minute); }
        public DateTime EndTime { get => StartTime.AddMinutes(Duration); }
        [Required]
        [Display(Name = "Duration of Reservation")]
        public int Duration { get; set; } //Minutes
        [Required]
        [Display(Name = "Number of Guests")]
        public int Guests { get; set; }
        [Display(Name = "Additional Notes")]
        public string Note { get; set; }


        //Relationships
        [Display(Name = "Status of Reservation")]
        public int ReservationStatusId { get; set; }
        public SelectList ReservationStatus { get; set; }
        [Required]
        [Display(Name = "Source of Reservation")]
        public int ReservationSourceId { get; set; }
        public SelectList ReservationSources { get; set; }
        [Required]
        [Display(Name = "Sitting of Reservation")]
        public int SittingId { get; set; }
        public SelectList Sittings { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Phone")]
        public string Phone { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Restaurant")]
        public int RestaurantId { get; set; }
        public SelectList Restaurants { get; set; }
    }
}
