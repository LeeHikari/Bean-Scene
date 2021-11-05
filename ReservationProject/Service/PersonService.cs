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
        private readonly UserManager<ApplicationUser> _userManager;


        public PersonService(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
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
                    UserId = _person.UserId
                };
                _context.People.Add(person);
            }
            else if(person != null && update)
            {
                person.Email = _person.Email.ToLower();
                person.FirstName = _person.FirstName;
                person.LastName = _person.LastName; 
                person.Phone = _person.Phone;

                if(person.UserId == null)
                {
                    person.UserId = _person.UserId;
                }
            }
            await _context.SaveChangesAsync();
            return person;
        }
    }
}
