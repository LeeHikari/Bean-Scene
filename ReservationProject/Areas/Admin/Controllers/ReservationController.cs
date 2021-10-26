using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReservationProject.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ReservationProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ReservationController : AdminAreaBaseController
    {
        public ReservationController(ApplicationDbContext context)
          : base(context)
        {

        }
        public async Task<IActionResult> Index()
        {
            var reservation = await _context.Reservations.OrderBy(reservation => reservation.Id).ToArrayAsync();
            return View(reservation);
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
                var reservation = new Reservation();
                {
                    reservation.StartTime = model.StartTime;
                    reservation.Duration = model.Duration;
                    reservation.Guests = model.Guests;
                    reservation.Note = model.Note;
                    reservation.ReservationSourceId = model.ReservationSourceId;
                    reservation.ReservationStatusId = 1;//pending
                    reservation.SittingId = model.SittingId;
                    reservation.RestaurantId = model.RestaurantId;
                    reservation.PersonId = model.PersonId;
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

