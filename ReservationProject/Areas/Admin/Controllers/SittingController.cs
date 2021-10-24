using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ReservationProject.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ReservationProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SittingController : AdminAreaBaseController
    {


        public SittingController(ApplicationDbContext context)
           : base(context)
        {

        }

        
            public IActionResult Index()
            {
                return View();
            }

            [HttpGet]
            public IActionResult Create()
            {

                var restaurantlist = _context.Restaurants.Select(r => new
                {
                    Value = r.Id,
                    Display = r.Name
                }).ToArray();

                var model = new Models.Sitting.Create

                {
                    Restaurants = new SelectList(restaurantlist.ToList(), "Value", "Display")
                };

                return View(model);
            }
            [HttpPost]
            public async Task<IActionResult> Create(Models.Sitting.Create model)
            {
                if (ModelState.IsValid)
                {
                var sitting = new Sitting { Name = model.Name, StartTime = model.StartTime, EndTime = model.EndTime, Capacity = model.Capacity, RestaurantId = model.RestaurantId, IsClosed = model.IsClosed };
                }
                return View(model);

            }
            public IActionResult Delete()
            {
                return View();
            }

            public IActionResult Update()
            {
                return View();
            }
    }
} 

