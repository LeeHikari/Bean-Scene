using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReservationProject.Data;

namespace ReservationProject.Areas.Member.Controllers
{

    [Authorize(Roles ="Member,Admin,Staff")]
    [Route("api/Reservations")]
    
    [ApiController]
    public class ReservationsApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReservationsApiController(ApplicationDbContext context, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser>userManager)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager; 
        }

        // GET: api/ReservationsControllerApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reservation>>> GetReservations()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Member"))
                {
                    var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

                    var reservations= await _context.Reservations
                        .Include(rs => rs.ReservationSource)
                        .Include(rst => rst.ReservationStatus)
                        .Include(sr => sr.Sitting.Restaurant)
                        .Include(s => s.Sitting)
                        .Include(p => p.Person)
                        .Where(p => p.Person.UserId == user.Id)
                        .ToListAsync();

                    if (reservations==null)
                    {
                        return NotFound();

                    }
                    return reservations;

                }
                return NotFound();

            }
            return NotFound();

        }

        // GET: api/ReservationsControllerApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Reservation>> GetReservation(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Member"))
                {
                    var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

                    var reservation = await _context.Reservations
                        .Include(rs => rs.ReservationSource)
                        .Include(rst => rst.ReservationStatus)
                        .Include(sr => sr.Sitting.Restaurant)
                        .Include(s => s.Sitting)
                        .Include(p => p.Person)
                        .Where(p => p.Person.UserId == user.Id)
                        .FirstAsync(r => r.Id == id);

                    if (reservation == null)
                    {
                        return NotFound();
                    }
                    return reservation;

                }

                return NotFound();

            }
            return NotFound();

        }

        // PUT: api/ReservationsControllerApi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReservation(int id, Reservation reservation)
        {
            if (id != reservation.Id)
            {
                return BadRequest();
            }

            _context.Entry(reservation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReservationExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ReservationsControllerApi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Reservation>> PostReservation(Reservation reservation)
        {
            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetReservation", new { id = reservation.Id }, reservation);
        }

        // DELETE: api/ReservationsControllerApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReservation(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }

            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ReservationExists(int id)
        {
            return _context.Reservations.Any(e => e.Id == id);
        }
    }
}
