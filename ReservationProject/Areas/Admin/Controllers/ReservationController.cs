﻿using Microsoft.AspNetCore.Authorization;
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
        public IActionResult Index()
        {
            return View();
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
                //upsert new person
                var p = new Person
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Phone = model.Phone

                };
                var person = await _personService.UpsertPersonAsync(p, true);


                //create reservation with persoon id
                var reservation = new Reservation();
                {
                    reservation.StartTime = model.StartTime;
                    reservation.Duration = model.Duration;
                    reservation.Guests = model.Guests;
                    reservation.Note = model.Note;
                    reservation.ReservationSourceId = 1;
                    reservation.ReservationStatusId = 1;//pending
                    reservation.SittingId = model.SittingId;
                    reservation.RestaurantId = 1;
                    reservation.PersonId = person.Id;
                }
                await _context.Reservations.AddAsync(reservation);
                await _context.SaveChangesAsync();


                return View(model);

            }
            return View(model);

        }
        public IActionResult Delete()
        {
            return View();
        }

        public IActionResult Update()
        {
            return View();
        }
    }
}

