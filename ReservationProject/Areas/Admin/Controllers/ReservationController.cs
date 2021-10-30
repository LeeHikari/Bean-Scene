using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReservationProject.Data;
using ReservationProject.Service;
using System.Linq;
using System.Threading.Tasks;

namespace ReservationProject.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Roles = "Admin, Staff")]
    public class ReservationController : AdminAreaBaseController
    {
        private readonly PersonService _personService;

        public ReservationController(ApplicationDbContext context, PersonService personService)
          : base(context)
        {
            _personService = personService;
        }
        public async Task<IActionResult> Index()
        {
            var reservation = await _context.Reservations.OrderBy(reservation => reservation.Id).ToArrayAsync();
            return View(reservation);
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {

            var sourceList = await _context.ReservationSources.Select(s => new
            {
                Value = s.Id,
                Display = s.Name
            }).ToArrayAsync();
            var restaurantList = await _context.Restaurants.Select(r => new
            {
                Value = r.Id,
                Display = r.Name
            }).ToArrayAsync();
            var sittingList = await _context.Sittings.Select(r => new
            {
                Value = r.Id,
                Display = r.Name
            }).ToArrayAsync();
            var model = new Models.Reservation.Create

            {
                ReservationSources = new SelectList(sourceList.ToList(), "Value", "Display"),               
                Restaurants = new SelectList(restaurantList.ToList(), "Value", "Display"),
                Sittings = new SelectList(sittingList.ToList(), "Value", "Display")

            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Create(Models.Reservation.Create model)
        {
            if (ModelState.IsValid)
            {
            
                var person = new Person
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email.ToLower(),
                    Phone = model.Phone
                };
                person = await _personService.UpsertPersonAsync(person, true);
                
                //TODO Ensure reservation time is within selected sitting time, else return error
               

                //TODO When you create a reservation it doesn't populate the select lists once it's created, so we need get all select lists so another reservation can be made

                //Create reservation with person Id
                var reservation = new Reservation();
                {
                    reservation.StartTime = model.StartTime;
                    reservation.Duration = model.Duration;
                    reservation.Guests = model.Guests;
                    reservation.Note = model.Note;
                    reservation.ReservationSourceId = model.ReservationSourceId;
                    reservation.ReservationStatusId = 1;//pending
                    reservation.SittingId = model.SittingId;
                    reservation.PersonId = person.Id;
                }
                _context.Reservations.Add(reservation);
                await _context.SaveChangesAsync();

            }
            return View(model);

        }
        public IActionResult Delete()
        {
            return View();
        }

        //TODO Parse Reservation status to Update

        public IActionResult Update()
        {
            return View();
        }
    }
}

