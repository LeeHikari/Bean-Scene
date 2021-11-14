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
        public async Task<IActionResult> Index(GetDate dateModel)
        {
            ViewData["Title"] = "Home Page";

            if (_context == null)
            {
                return View();
            }

            var m = new Models.MultiViewModel();
            m.Create = new Models.Home.Index();

            if (dateModel.ObtainDate != new DateTime())
            {
                ViewData["FormDate"] = dateModel.ObtainDate.ToString("yyyy-MM-dd");

                m.Create.ResDate = dateModel.ObtainDate;

                var date = dateModel.ObtainDate.Date;

                var availableSittings = _context.Sittings.Where(s => s.StartTime.Date == date.Date);
                if (availableSittings != null)
                {
                    m.Create.SittingTypes = new SelectList(availableSittings, "Id", "Name");
                }
                else
                {
                    var sittingTypeOptions = _context.Sittings.Select(s => new
                    {
                        Value = s.Id,
                        Display =
                    $"{s.Name} - " +
                    $"Start: {s.StartTime.ToString("hh:mm")} - " +
                    $"End: {s.EndTime.ToString("hh:mm")} - " +
                    $"Spots left: {s.Vacancies}"
                    }).ToArray();

                    m.Create.SittingTypes = new SelectList(sittingTypeOptions, "Value", "Display");
                }


            }
            else
            {
                var sittingTypeOptions = _context.Sittings.Select(s => new
                {
                    Value = s.Id,
                    Display =
                    $"{s.Name} - " +
                    $"Start: {s.StartTime.ToString("hh:mm")} - " +
                    $"End: {s.EndTime.ToString("hh:mm")} - " +
                    $"Spots left: {s.Vacancies}"
                }).ToArray();

                m.Create.SittingTypes = new SelectList(sittingTypeOptions, "Value", "Display");

            }
            //TODO: check if member and not other type of user
            if (User.Identity.IsAuthenticated)
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

                var person = await _context.People.FirstOrDefaultAsync(p => p.Email == user.Email);

                if(person != null)
                {
                    m.Create.FirstName = person.FirstName;
                    m.Create.LastName = person.LastName;
                    m.Create.Email = person.Email;
                    m.Create.Phone = person.Phone;

                    return View(m);
                }
                else
                {
                    m.Create.FirstName = "";
                    m.Create.LastName = "";
                    m.Create.Email = "";
                    m.Create.Phone = "";

                    return View(m);
                }
            }

            m.Create.FirstName = "";
            m.Create.LastName = "";
            m.Create.Email = "";
            m.Create.Phone = "";


            return View(m);
        }


        [HttpPost, ActionName("Index")]
        public async Task<IActionResult> CreateConfirmed(MultiViewModel model)
        {

            var person = await _context.People.FirstOrDefaultAsync(p => p.Email == model.Create.Email);

            if (ModelState.IsValid)
            {

                
                    person = new Person
                    {
                        Email = model.Create.Email,
                        FirstName = model.Create.FirstName,
                        LastName = model.Create.LastName,
                        Phone = model.Create.Phone
                    };
                    person = await _personService.UpsertPersonAsync(person,true);
                
             
                var reservation = new Reservation
                {
                    StartTime = model.Create.StartTime,
                    Duration = model.Create.Duration,
                    PersonId = person.Id,
                    Guests = model.Create.Guests,
                    ReservationSourceId = 1,
                    ReservationStatusId = 2,
                    SittingId = model.Create.SittingId,
                    Note = model.Create.Note


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

            model.Create.SittingTypes = new SelectList(_context.Sittings.ToArray(), nameof(Sitting.Id), nameof(Sitting.Name));
            return View(model);
        }

        [HttpPost]
        public IActionResult FindSitting(Models.MultiViewModel m) //Requires MultiViewModel's - Create VM
        {
            GetDate getDate = new GetDate { ObtainDate = m.GetDate.ObtainDate };
            return RedirectToAction("Index", "Home", getDate);
        }

        [HttpGet]
        public async Task<IActionResult> Confirm(int? id)
        {
            try
            {
                if (!id.HasValue)
                {
                    return NotFound();
                }

                var reservation = await _context.Reservations
                    .Include(rs => rs.ReservationSource)
                    .Include(rst => rst.ReservationStatus)
                    .Include(sr => sr.Sitting.Restaurant)
                    .Include(s => s.Sitting)
                    .Include(p => p.Person)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(r => r.Id == id);

                if (reservation == null)
                {
                    return NotFound();
                }

                return View(reservation);
            }
            catch (Exception)
            {

            }
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
