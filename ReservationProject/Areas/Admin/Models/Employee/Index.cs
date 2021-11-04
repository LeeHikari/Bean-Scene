using ReservationProject.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReservationProject.Areas.Admin.Models.Employee
{
    public class Index:Person
    {
        public List<Person> Staff { get; set; } = new List<Person>();
        public List<Person> Admin { get; set; } = new List<Person>();
    }
}
