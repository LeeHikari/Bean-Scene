using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReservationProject.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReservationProject.Controllers
{
    public class ReservationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ReservationController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        //TODO 8/19/21 9:31am, 38 minutes
        [HttpGet]
        public IActionResult Create()
        {
            var model = new Models.Reservation.Create
            {
                //TODO (Group)
                ////_context.Sittings to an array
                // Return the Filtered array for specified day -calendar linked
                // 1/10/21
                // Get non locked sittings.
                // return as the source for the select list.

                SittingTypes = new SelectList(_context.Sittings.ToArray(), nameof(Sitting.Id), nameof(Sitting.Name))

                //Calendar - Automatically Lock day, if no sittings assigned.

                // We will create a edit page specifically for admin usage, that can select a date on the calendar
                //create, update, delete spe

                //personal details automatically assigned using 'asp-for="FirstName"' Or however we've named it exactly in the model.

                //Summary 
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Models.Reservation.Create model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var reservation = _mapper.Map<Data.Reservation>(model);
                    _context.Reservations.Add(reservation);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ex.Message.ToString();
                }
            }

            model.SittingTypes = new SelectList(_context.Sittings.ToArray(), nameof(Sitting.Id), nameof(Sitting.Name));
            return View(model);
        }
    }
}
