using Microsoft.AspNetCore.Mvc.Rendering;
using ReservationProject.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReservationProject.Areas.Admin.Models.Employee
{
    public class Update 
    {
        public Update(ApplicationUser employee)
        {
            Id = employee.Id;
            FirstName = employee.FirstName;
            LastName = employee.LastName;
            Email = employee.Email;
            Phone = employee.PhoneNumber;

        }
        public Update()
        {

        }
        //Id
        [Required]
        public string Id { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }


        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Phone { get; set; }

        public SelectList Roles { get; set; }
        [Display(Name = "Role")]
        [Required(ErrorMessage = "Employee - Role: Required")]
        public string Role { get; set; }

    }
}
