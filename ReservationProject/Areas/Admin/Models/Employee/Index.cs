using ReservationProject.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReservationProject.Areas.Admin.Models.Employee
{
    public class Index:Person
    {
        public IList<ApplicationUser> Staff { get; set; } = new List<ApplicationUser>();
        public IList<ApplicationUser> Admin { get; set; } = new List<ApplicationUser>(); 
       public IList<ApplicationUser> Deactivate { get; set; } = new List<ApplicationUser>();
    }
}
