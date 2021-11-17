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

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var sittings = await _context.Sittings
                        .Include(r=>r.Reservations)
                        .OrderBy(sitting => sitting.StartTime).ToArrayAsync();

            List<Models.Report.Index> models = new List<Models.Report.Index>();
            foreach (var sitting in sittings)
            {
                models.Add(new Models.Report.Index 
                { 
                    Id = sitting.Id,
                    Name = sitting.Name,
                    StartTime = sitting.StartTime,
                    EndTime = sitting.EndTime,
                    Capacity = sitting.Capacity,
                    Reservations = sitting.Reservations,
                
                });;
            }
            return View(models);
        }
    }
}
