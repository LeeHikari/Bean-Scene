using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReservationProject.Data;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ReservationProject.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Roles = "Admin")]
    public class SittingController : AdminAreaBaseController
    {


        public SittingController(ApplicationDbContext context)
           : base(context)
        {

        }

        public async Task<IActionResult> Index()
        {
            var sitting = await _context.Sittings
                .Include(r => r.Restaurant)
                .OrderBy(sitting => sitting.Id).ToArrayAsync();
            return View(sitting);
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
                try
                {
                    var sitting = new Sitting();
                    {
                        sitting.Name = model.Name;
                        sitting.StartTime = model.StartTime;
                        sitting.EndTime = model.EndTime;
                        sitting.Capacity = model.Capacity;
                        sitting.RestaurantId = model.RestaurantId;
                        sitting.IsClosed = model.IsClosed;
                    }
                    await _context.Sittings.AddAsync(sitting);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    StatusCode(500);
                }
            }
            return View(model);

        }
        public IActionResult Delete()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {

            if (!id.HasValue)
            {
                return NotFound();
            }

            var sitting = await _context.Sittings
                .Include(r => r.Restaurant)
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == id);

            if (sitting == null)
            {
                return NotFound();
            }

            return View(sitting);
        }

        public IActionResult Update()
        {
            return View();
        }
    }
} 


