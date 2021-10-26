using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ReservationProject.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReservationProject.Service
{
    public class PersonService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;


        public PersonService(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        public async Task<Person> UpsertPersonAsync(Person _person, bool update)
        {
            var person =  await _context.People.FirstOrDefaultAsync(p => p.Email == _person.Email);
            if (person == null)
            {
                person = new Person
                {
                    Email = _person.Email,
                    FirstName = _person.FirstName,
                    LastName = _person.LastName,
                    Phone = _person.Phone,
                };
                _context.People.Add(person);
            }
            else if(person != null && update)
            {
                person.Email = _person.Email;
                person.FirstName = _person.FirstName;
                person.LastName = _person.LastName; 
                person.Phone = _person.Phone;
            }
            await _context.SaveChangesAsync();
            return person;
        }
    }
}
