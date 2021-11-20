using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReservationProject.Areas.Admin.Models.Sitting
{
    public class CreateMulti:Create
    {
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Sitting Date End")]
        public DateTime SitDate2 { get; set; }
        [Required]
        public int Recurrence { get; set; }

    }
}
