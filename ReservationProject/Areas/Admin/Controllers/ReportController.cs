using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReservationProject.Data;

namespace ReservationProject.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Roles = "Admin")]
    public class ReportController : AdminAreaBaseController
    {
        public ReportController(ApplicationDbContext context)
         : base(context)
        {

        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
