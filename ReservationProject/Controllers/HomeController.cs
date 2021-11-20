using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ReservationProject.Data;
using ReservationProject.Models;
using ReservationProject.Models.Home;
using ReservationProject.Service;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ReservationProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly PersonService _personService;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<ApplicationUser> userManager, IMapper mapper, PersonService personService)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
            _personService = personService;

        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Home Page";

            if (_context == null)
            {
                return View();
            }

            var m = new Models.Home.Index();

                var sittingTypeOptions = _context.Sittings.Select(s => new
                {
                    Value = s.Id,
                    Display =
                    $"{s.Name} - " +
                    $"Start: {s.StartTime.ToString("hh:mm")} - " +
                    $"End: {s.EndTime.ToString("hh:mm")} - " +
                    $"Spots left: {s.Vacancies}"
                }).ToArray();

                m.SittingTypes = new SelectList(sittingTypeOptions, "Value", "Display");

            //}
            //TODO: check if member and not other type of user
            if (User.Identity.IsAuthenticated)
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

                var person = await _context.People.FirstOrDefaultAsync(p => p.Email == user.Email);

                if(person != null)
                {
                    m.FirstName = person.FirstName;
                    m.LastName = person.LastName;
                    m.Email = person.Email;
                    m.Phone = person.Phone;

                    return View(m);
                }
                else
                {
                    m.FirstName = "";
                    m.LastName = "";
                    m.Email = "";
                    m.Phone = "";

                    return View(m);
                }
            }

            m.FirstName = "";
            m.LastName = "";
            m.Email = "";
            m.Phone = "";


            return View(m);
        }


        [HttpPost, ActionName("Index")]
        public async Task<IActionResult> CreateConfirmed(Models.Home.Index model)
        {

            var person = await _context.People.FirstOrDefaultAsync(p => p.Email == model.Email);

            if (ModelState.IsValid)
            {

                
                    person = new Person
                    {
                        Email = model.Email,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Phone = model.Phone
                    };
                    person = await _personService.UpsertPersonAsync(person,true);
                
             
                var reservation = new Reservation
                {
                    StartTime = model.StartTime,
                    Duration = model.Duration,
                    PersonId = person.Id,
                    Guests = model.Guests,
                    ReservationSourceId = 1,
                    ReservationStatusId = 1,
                    SittingId = model.SittingId,
                    Note = model.Note

                };

                _context.Reservations.Add(reservation);

                await _context.SaveChangesAsync();
                var confirmReservation = await _context.Reservations
                    .Include(rs => rs.ReservationSource)
                    .Include(rst => rst.ReservationStatus)
                    .Include(sr => sr.Sitting.Restaurant)
                    .Include(s => s.Sitting)
                    .Include(p => p.Person)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(r => r.Id == reservation.Id);
                if (confirmReservation == null)
                {
                    return NotFound();
                }
                return View("Confirm", confirmReservation);

            }

            model.SittingTypes = new SelectList(_context.Sittings.ToArray(), nameof(Sitting.Id), nameof(Sitting.Name));
            return View(model);
        }

        [HttpGet]
        public IActionResult Confirm(Reservation m)
        {
            try
            {
                if (m == null)
                {
                    return NotFound();
                }
                return View(m);
            }
            catch (Exception){ }

            return View();

        }

        [HttpPost]
        public JsonResult GetTimes(int id)
        {
            Task<Sitting> SittingTimes = _context.Sittings.FirstOrDefaultAsync(s => s.Id == id);
            SittingTimes.Wait();
            var sitting = SittingTimes.Result;

            List<DateTime> dates = new List<DateTime>();

            if (sitting != null)
            {
                dates.Add(sitting.StartTime);
                dates.Add(sitting.EndTime);
            }
            return Json(dates);
        }

        [HttpPost]
        public JsonResult GetSittings(string date)
        {
            var sittingsList = _context.Sittings.Where(s => s.StartTime.Date == DateTime.Parse(date).Date);

            return Json(sittingsList);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
