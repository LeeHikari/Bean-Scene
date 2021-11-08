using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReservationProject.Data;
using ReservationProject.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReservationProject.Areas.Member.Controllers
{
    [Area("Member"), Authorize(Roles = "Member")]

    public class ReservationController : MemberAreaBaseController
    {
        private readonly PersonService _personService;


        public ReservationController(ApplicationDbContext context, PersonService personService)
          : base(context)
        {
            _personService = personService;
        }
        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Member"))
                {
                    var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

                    var reservation = await _context.Reservations
                      .Include(rs => rs.ReservationSource)
                      .Include(rst => rst.ReservationStatus)
                      .Include(sr => sr.Sitting.Restaurant)
                      .Include(s => s.Sitting)
                      .Include(p => p.Person)
                      .Where(p => p.Person.UserId == user.Id)
                      .ToArrayAsync();

                    return View(reservation);
                }
                return View();
            }

            return View();

        }
    }
}
