using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReservationProject.Data;

namespace ReservationProject.Areas.Admin.Controllers
{
  
    [Area("Admin")/*, Authorize(Roles = "Employee, Manager,Admin")*/]
    public abstract class AdminAreaBaseController : Controller
    {
        protected readonly ApplicationDbContext _context;

        public AdminAreaBaseController(ApplicationDbContext context)
        {
            _context = context;
        }
    }
}