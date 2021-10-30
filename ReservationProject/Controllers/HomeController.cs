using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ReservationProject.Data;
using ReservationProject.Models;
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
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMapper _mapper;
        private readonly PersonService _personService;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<IdentityUser> userManager, IMapper mapper, PersonService personService)
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
            var sittingTypeOptions = await _context.Sittings.Select(s => new
            {
                Value = s.Id,
                Display = $"{s.Name} {s.StartTime.ToString("h:mm tt")}-{s.EndTime.ToString("h:mm tt")} Spaces Left "
            })
            .ToArrayAsync(); 
            var model = new Models.Home.Index
            {
                SittingTypes = new SelectList(sittingTypeOptions, "Value", "Display")
            };
            //TODO: check if member and not other type of user
            if (User.Identity.IsAuthenticated)
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

                var person = await _context.People.FirstOrDefaultAsync(p => p.Email == user.Email);

                if(person != null)
                {
                    model.FirstName = person.FirstName;
                    model.LastName = person.LastName;
                    model.Email = person.Email;
                    model.Phone = person.Phone;

                    return View(model);
                }
                else
                {
                    model.FirstName = "";
                    model.LastName = "";
                    model.Email = "";
                    model.Phone = "";

                    return View(model);
                }
            }

            model.FirstName = "";
            model.LastName = "";
            model.Email = "";
            model.Phone = "";


            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Models.Home.Index model)
        {
            //Person person = null;
            //if (User.Identity.IsAuthenticated)
            //{
            //    var user = await _userManager.FindByNameAsync(User.Identity.Name);
            //    person = await _context.People.FirstOrDefaultAsync(p => p.UserId == user.Id);
            //    ModelState.Remove("Email");
            //    ModelState.Remove("FirstName");
            //    ModelState.Remove("LastName");
            //    ModelState.Remove("Phone");
            //}

            var person = await _context.People.FirstOrDefaultAsync(p => p.Email == model.Email);
            //if (person.UserId != null)
            //{
            //    //TODO add code advise they must log in
            //}

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
                
             
                //create new reservation assign the person id

                var reservation = new Reservation
                {
                    StartTime = model.StartTime,
                    Duration = model.Duration,
                    PersonId = person.Id,
                    Guests = model.Guests,
                    ReservationSourceId = 1,
                    ReservationStatusId = 2,
                    SittingId = model.SittingId,
                    Note = model.Note


                };

                _context.Reservations.Add(reservation);
                await _context.SaveChangesAsync();
            }

            model.SittingTypes = new SelectList(_context.Sittings.ToArray(), nameof(Sitting.Id), nameof(Sitting.Name));
            return View(model);
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
