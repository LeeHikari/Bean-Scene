using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReservationProject.Models.Member
{
    public class Create
    {
        [Required(ErrorMessage = "Lastname: Required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Firstname: Required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Email: Required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone: Required")]
        public string Phone { get; set; }
    }
}
