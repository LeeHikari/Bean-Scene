using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReservationProject.Areas.Admin.Models.Employee
{
    public class Update : Models.Employee.Create
    {
        //Id
        [Required]
        public string Id { get; set; }

    }
}
