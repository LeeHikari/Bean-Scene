using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReservationProject.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReservationProject.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Roles = "Admin")]
    public class ReportController : AdminAreaBaseController
    {
        public ReportController(ApplicationDbContext context)
         : base(context)
        {

        }

        //TODO Report Controller

        public async Task<IActionResult> Index()
        {
            var sittings = await _context.Sittings
                        .Include(r => r.Restaurant)
                        .Include(r=>r.Reservations)
                        .OrderBy(sitting => sitting.StartTime).ToArrayAsync();
            var model = new Models.Report.Index();
            return View(model);
        }
    }
}
