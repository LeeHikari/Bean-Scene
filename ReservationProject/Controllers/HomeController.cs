using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ReservationProject.Data;
using ReservationProject.Models;
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

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<IdentityUser> userManager, IMapper mapper)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _mapper = mapper;

        }

        public async Task<IActionResult> Index()
        {
            var model = new Models.Home.Index
            {
                SittingTypes = new SelectList(_context.Sittings.ToArray(), nameof(Sitting.Id), nameof(Sitting.Name))
            };
            //TODO: check if member and not other type of user
            if (User.Identity.IsAuthenticated)
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

                var person = await _context.People.FirstOrDefaultAsync(p => p.UserId == user.Id);

                //Edit Code, Instead of repeatedly calling the database, we're calling it above in person
                //and using the person object to assign the values below.

                model.FirstName = person.FirstName;
                model.LastName = person.LastName;
                model.Email = person.Email;
                model.Phone = person.Phone;

                return View(model);
            }

            model.FirstName = "";
            model.LastName = "";
            model.Email = "";
            model.Phone = "";


            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(Models.Home.Index model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var reservation = _mapper.Map<Data.Reservation>(model);
                    _context.Reservations.Add(reservation);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ex.Message.ToString();
                }
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
