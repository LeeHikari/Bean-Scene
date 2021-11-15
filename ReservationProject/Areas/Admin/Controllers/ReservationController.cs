using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ReservationProject.Data;
using ReservationProject.Service;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ReservationProject.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Roles = "Admin, Staff")]
    public class ReservationController : AdminAreaBaseController
    {
        private readonly PersonService _personService;
        private readonly ILogger<ReservationController> _logger;

        public ReservationController(ApplicationDbContext context, PersonService personService, ILogger<ReservationController> logger)
          : base(context)
        {
            _personService = personService;
            _logger = logger;
        }
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Reservation Index at {time}", DateTime.UtcNow);
            Trace.Listeners.Add(new TextWriterTraceListener("MyOutput.log"));
            var reservation = await _context.Reservations
                .Include(rs => rs.ReservationSource)
                .Include(rst => rst.ReservationStatus)
                .Include(sr => sr.Sitting.Restaurant)
                .Include(s => s.Sitting)
                .Include(p => p.Person)
                .OrderBy(reservation => reservation.Id)
                .ToArrayAsync();
            Debug.Assert(reservation is not null, "Reservation is not null");
            Trace.Close();
            

            return View(reservation);
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            _logger.LogInformation("[HttpGet] Reservation at {time}", DateTime.UtcNow);

            var sourceList = await _context.ReservationSources.Select(s => new
            {
                Value = s.Id,
                Display = s.Name
            }).ToArrayAsync();

            var restaurantList = await _context.Restaurants.Select(r => new
            {
                Value = r.Id,
                Display = r.Name
            }).ToArrayAsync();

            var sittingList = await _context.Sittings.Select(r => new
            {
                Value = r.Id,
                Display = $"{r.Name} {r.StartTime.ToString("h:mm tt")} - {r.EndTime.ToString("h:mm tt")}"
            }).ToArrayAsync();

            _logger.LogInformation("Parsing in the create model for Reservations to render SelectLists");
            var model = new Models.Reservation.Create

            {
                ReservationSources = new SelectList(sourceList.ToList(), "Value", "Display"),               
                Restaurants = new SelectList(restaurantList.ToList(), "Value", "Display"),
                Sittings = new SelectList(sittingList.ToList(), "Value", "Display")

            };

            

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Create(Models.Reservation.Create model)
        {
            _logger.LogInformation("[HttpPost] Reservation at {time}", DateTime.UtcNow);
            if (ModelState.IsValid)
            {
                try
                {
                    var sitting = await _context.Sittings.FindAsync(model.SittingId);
                    if (sitting.IsClosed == false)
                    {
                        var person = new Person
                        {
                            FirstName = model.FirstName,
                            LastName = model.LastName,
                            Email = model.Email.ToLower(),
                            Phone = model.Phone
                        };
                        person = await _personService.UpsertPersonAsync(person, true);

                        //TODO Ensure reservation time is within selected sitting time, else return error

                        //get date
                        //return relevant sittings
                        //otherwise select sitting
                        //return select times



                        //Create reservation with person Id
                        var reservation = new Reservation();
                        {
                            reservation.StartTime = model.StartTime;
                            reservation.Duration = model.Duration;
                            reservation.Guests = model.Guests;
                            reservation.Note = model.Note;
                            reservation.ReservationSourceId = model.ReservationSourceId;
                            reservation.ReservationStatusId = 1;//pending
                            reservation.SittingId = model.SittingId;
                            reservation.PersonId = person.Id;
                        }
                        _context.Reservations.Add(reservation);
                        await _context.SaveChangesAsync();
                        _logger.LogInformation("Submit Reservation {time}", DateTime.UtcNow);

                        return RedirectToAction(nameof(Index));

                    }
                }
                catch(Exception)
                {
                    StackTrace st = new StackTrace(true);
                    for (int i = 0; i < st.FrameCount; i++)
                    {
                        StackFrame sf = st.GetFrame(i);
                        Console.WriteLine();
                        Console.WriteLine("High up the call stack, Method {0}");
                        sf.GetMethod();

                        Console.WriteLine("High up the call stack, Line Number: {0}");
                        sf.GetFileName();
                    }


                    StatusCode(500);
                }

            }

            //return selectlist data after create
            var sourceList = await _context.ReservationSources.Select(s => new
            {
                Value = s.Id,
                Display = s.Name
            }).ToArrayAsync();
            var restaurantList = await _context.Restaurants.Select(r => new
            {
                Value = r.Id,
                Display = r.Name
            }).ToArrayAsync();
            var sittingList = await _context.Sittings.Select(r => new
            {
                Value = r.Id,
                Display = r.Name
            }).ToArrayAsync();
            model = new Models.Reservation.Create

            {
                ReservationSources = new SelectList(sourceList.ToList(), "Value", "Display"),
                Restaurants = new SelectList(restaurantList.ToList(), "Value", "Display"),
                Sittings = new SelectList(sittingList.ToList(), "Value", "Display")

            };
            return View(model);

        }

        //TODO Parse Reservation status to Update

        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {

            try
            {
                _logger.LogWarning("If Reservation Id is equal to null, return to StatusCode 400");
                if(id == null)
                {
                    return StatusCode(400, "Id Required");
                }


                var reservation = await _context.Reservations
                    .Include(rs => rs.ReservationSource)
                    .Include(rst => rst.ReservationStatus)
                    .Include(sr => sr.Sitting.Restaurant)
                    .Include(s => s.Sitting)
                    .Include(p => p.Person)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(r => r.Id == id);

                if (reservation == null)
                {
                    return NotFound();
                }
                var reservationStatusOptions = await _context.ReservationStatuses.Select(rs => new
                {
                    Value = rs.Id,
                    Display = rs.Name
                })
                .ToArrayAsync();

                var sourceList = await _context.ReservationSources.Select(s => new
                {
                    Value = s.Id,
                    Display = s.Name
                }).ToArrayAsync();

                var sittingList = await _context.Sittings.Select(r => new
                {
                    Value = r.Id,
                    Display = $"{r.Name} {r.StartTime.ToString("h:mm tt")} - {r.EndTime.ToString("h:mm tt")}"
                }).ToArrayAsync();

                var restaurantList = await _context.Restaurants.Select(r => new
                {
                    Value = r.Id,
                    Display = r.Name
                }).ToArrayAsync();

                var model = new Models.Reservation.Update(reservation)
                {   
                    ReservationStatuses = new SelectList(reservationStatusOptions.ToList(), "Value", "Display"),
                    Restaurants = new SelectList(restaurantList.ToList(), "Value", "Display"),
                    ReservationSources = new SelectList(sourceList.ToList(), "Value", "Display"),
                    Sittings = new SelectList(sittingList.ToList(), "Value", "Display")
                };

                return View(model);

            }
            catch(Exception)
            {
                return StatusCode(500);
            }
            //if (!id.HasValue)
            //{
            //    return NotFound();
            //}
            //var reservation = await _context.Reservations.FirstOrDefaultAsync(r => r.Id == id);
            //if (ModelState.IsValid)
            //{
            //    try
            //    {
            //        _context.Update(reservation);
            //        await _context.SaveChangesAsync();
            //        return RedirectToAction("Update");
            //    }
            //    catch (DbUpdateException)
            //    {
            //        ModelState.AddModelError("", "Unable to save changes");
            //    }
            //}

            //var person = await _context.People.FirstOrDefaultAsync(p => p.Id == reservation.PersonId);
            //if (person == null)
            //{
            //    return NotFound();
            //}




            
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {

            if (!id.HasValue)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .Include(rs => rs.ReservationSource)
                .Include(rst => rst.ReservationStatus)
                .Include(sr => sr.Sitting.Restaurant)
                .Include(s => s.Sitting)
                .Include(p => p.Person)
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }
    }
}

