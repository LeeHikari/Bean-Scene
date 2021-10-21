using Microsoft.AspNetCore.Mvc;

namespace ReservationProject.Areas.Admin.Controllers
{
    public class SittingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
