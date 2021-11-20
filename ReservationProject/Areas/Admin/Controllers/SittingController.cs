using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReservationProject.Data;
using System;
using System.Collections.Generic;
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
        public IActionResult CreateMulti()
        {
            var restaurantlist = _context.Restaurants.Select(r => new
            {
                Value = r.Id,
                Display = r.Name
            }).ToArray();

            var model = new Models.Sitting.CreateMulti

            {
                Restaurants = new SelectList(restaurantlist.ToList(), "Value", "Display")
            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> CreateMulti(Models.Sitting.CreateMulti model)
        {
         
            if (ModelState.IsValid)
            {
                try
                {
                    List <DateTime>allDates = new List<DateTime>();
                    for (DateTime date = model.SitDate /*startdate*/; date <= model.SitDate2/*enddate*/; date = date.AddDays(1))
                        allDates.Add(date);

                    foreach (DateTime date in allDates)
                    {
                        //All Days
                        if (model.Recurrence == 1)
                        {
                            var sitting = new Sitting();
                            {
                                sitting.Name = model.Name;
                                sitting.StartTime = date.AddHours(model.SitStartTime.Hour).AddMinutes(model.SitStartTime.Minute); 
                                sitting.EndTime = date.AddHours(model.SitEndTime.Hour).AddMinutes(model.SitEndTime.Minute);
                                sitting.Capacity = model.Capacity;
                                sitting.RestaurantId = model.RestaurantId;
                                sitting.IsClosed = model.IsClosed;
                            }
                            await _context.Sittings.AddAsync(sitting);
                            await _context.SaveChangesAsync();
                        }
                        //Weekdays
                        if (model.Recurrence == 2)
                        {
                            if (date.DayOfWeek!=DayOfWeek.Saturday&& date.DayOfWeek != DayOfWeek.Sunday)
                            {
                                var sitting = new Sitting();
                                {
                                    sitting.Name = model.Name;
                                    sitting.StartTime = date.AddHours(model.SitStartTime.Hour).AddMinutes(model.SitStartTime.Minute)
;                                   sitting.EndTime = date.AddHours(model.SitEndTime.Hour).AddMinutes(model.SitEndTime.Minute);
                                    sitting.Capacity = model.Capacity;
                                    sitting.RestaurantId = model.RestaurantId;
                                    sitting.IsClosed = model.IsClosed;
                                }
                                await _context.Sittings.AddAsync(sitting);
                                await _context.SaveChangesAsync();

                            }
                           
                        }
                        //WeekEnds
                        if (model.Recurrence == 3)
                        {
                            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
                            {
                                var sitting = new Sitting();
                                {
                                    sitting.Name = model.Name;
                                    sitting.StartTime = date.AddHours(model.SitStartTime.Hour).AddMinutes(model.SitStartTime.Minute);                                   
                                    sitting.EndTime = date.AddHours(model.SitEndTime.Hour).AddMinutes(model.SitEndTime.Minute);
                                    sitting.Capacity = model.Capacity;
                                    sitting.RestaurantId = model.RestaurantId;
                                    sitting.IsClosed = model.IsClosed;
                                }
                                await _context.Sittings.AddAsync(sitting);
                                await _context.SaveChangesAsync();
                            }
                        }
                    }
                   

                                      
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    StatusCode(500);
                }
            }
            return View(model);
        }

        private IEnumerable<DateTime> EachDay(DateTime sitDate, DateTime sitDate2)
        {
            throw new NotImplementedException();
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

                s.StartTime = model.SitStartTime;
                s.EndTime = model.SitEndTime;
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


