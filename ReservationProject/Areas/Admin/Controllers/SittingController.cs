using AutoMapper;
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
        private IMapper _mapper;

        public SittingController(IMapper mapper, ApplicationDbContext context)
           : base(context)
        {
            _mapper=mapper;
        }

        public async Task<IActionResult> Index()
        {
            var sitting = await _context.Sittings
                .Include(r => r.Restaurant)
                .OrderBy(sitting => sitting.StartTime).ToArrayAsync();
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
                        sitting.Name = model.Name + " " + model.StartTime.ToString("h:mm tt") + "-" + model.EndTime.ToString("h:mm tt");
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
        [HttpGet]

        public async Task<IActionResult> Update(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound();
            }

            var sitting = await _context.Sittings
                .Include(r => r.Restaurant)
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == id);
            var restaurantlist = _context.Restaurants.Select(r => new
            {
                Value = r.Id,
                Display = r.Name
            }).ToArray();

            if (sitting == null)
            {
                return NotFound();
            }
            var model = new Models.Sitting.Update

            { Id = sitting.Id,
                Name = sitting.Name,
                SitDate = sitting.StartTime,
                SitStartTime = sitting.StartTime,
                SitEndTime = sitting.EndTime,
                Capacity = sitting.Capacity,
                RestaurantId = sitting.RestaurantId,
                IsClosed = sitting.IsClosed,

                Restaurants = new SelectList(restaurantlist.ToList(), "Value", "Display")
            };

            return View(model);
        }
        [HttpPost]

        public async Task<IActionResult> Update(int id, Models.Sitting.Update model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }
            var restaurantlist = _context.Restaurants.Select(r => new
            {
                Value = r.Id,
                Display = r.Name
            }).ToArray();

            model.Restaurants =new SelectList( restaurantlist,"Value", "Display");
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
              

       
                var s = _mapper.Map<Data.Sitting>(model);
                _context.Update<Sitting>(s);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
    } 
} 


