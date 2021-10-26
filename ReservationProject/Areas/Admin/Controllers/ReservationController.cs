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
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public IActionResult Create()
        {

            var sourceList = _context.ReservationSources.Select(s => new
            {
                Value = s.Id,
                Display = s.Name
            }).ToArray();
            var restaurantList = _context.Restaurants.Select(r => new
            {
                Value = r.Id,
                Display = r.Name
            }).ToArray();
            var sittingList = _context.Sittings.Select(r => new
            {
                Value = r.Id,
                Display = r.Name
            }).ToArray();
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
                var person= _personService.UpsertPersonAsync(p, true);


                //create reservation with persoon id
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
                _context.SaveChanges();


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

