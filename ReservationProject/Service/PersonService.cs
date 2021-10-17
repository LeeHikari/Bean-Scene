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


        public async Task<Person> UpsertPersonAsync(Person data, bool update)
        {
            var person = await _context.People.FirstOrDefaultAsync(p => p.Email == data.Email);
            if (person == null)
            {
                person = new Person
                {
                    Email = data.Email,
                    FirstName = data.FirstName,
                    LastName = data.LastName,
                    Phone = data.Phone,
                    UserId = data.UserId
                };
                _context.People.Add(person);
               
            }
            if(person != null && update)
            {
                person.Email = data.Email;
                person.FirstName = data.FirstName;
                person.LastName = data.LastName; 
                person.Phone = data.Phone;
                person.UserId = data.UserId;
            }
            await _context.SaveChangesAsync();
            return person;
        }
    }
}
