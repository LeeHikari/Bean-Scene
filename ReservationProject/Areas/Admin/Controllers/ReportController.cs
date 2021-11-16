using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReservationProject.Data;
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
            var sitting = await _context.Sittings
                        .Include(r => r.Restaurant)
                        .OrderBy(sitting => sitting.StartTime).ToArrayAsync();
            return View(sitting);
        }
    }
}
