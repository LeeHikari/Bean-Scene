using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ReservationProject.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.WebPages.Html;

namespace ReservationProject.Controllers
{
    public class ReservationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private Mapper _mapper;

        public ReservationController(ApplicationDbContext context)
        {
            _context = context;
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Models.Reservation.Create, Data.Sitting>());
            _mapper = new Mapper(config);
        }


        public IActionResult Index()
        {
            return View();
        }

        //TODO 8/19/21 9:31am, 38 minutes
        [HttpGet]
        public IActionResult Create()
        {
            var model = new Models.Reservation.Create
            {
                Reservations = new SelectList(_context.Sittings.ToArray(), nameof(Sitting.Name), nameof(Sitting.StartTime))
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
                    var reservation = _mapper.Map<Data.Sitting>(model);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ex.Message.ToString();
                }
            }

            model.Reservations = new SelectList(_context.Sittings.ToArray(), nameof(Sitting.Name), nameof(Sitting.StartTime));
            return View(model);
        }
    }
}
