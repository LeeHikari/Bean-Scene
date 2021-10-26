using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReservationProject.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReservationProject.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Roles = "Admin, Staff")]
    public class HomeController : AdminAreaBaseController
    {
        public HomeController(ApplicationDbContext context) : base(context) { }

        public IActionResult Index()
        {
            return View();
        }
    }
}
