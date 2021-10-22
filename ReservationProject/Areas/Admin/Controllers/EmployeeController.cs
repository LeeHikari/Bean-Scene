using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReservationProject.Data;
using ReservationProject.Service;
using System.Linq;
using System.Threading.Tasks;

namespace ReservationProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class EmployeeController : AdminAreaBaseController
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly PersonService _personService;

        public EmployeeController(ApplicationDbContext context, UserManager<IdentityUser> userManager, PersonService personService)
            : base(context)
        {
            _userManager = userManager;
            _personService = personService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            //var roleOptions = _context.UserRoles.

            var model = new Models.Employee.Create
            {
                Roles = new SelectList(_context.UserRoles.ToArray(), "Id", "Name")
            };
            return View(model);
        }

        public async Task<IActionResult> Create(Models.Employee.Create model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = model.Email, Email = model.Email, PhoneNumber = model.Phone };
                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Staff");

                    var p = new Person();
                    {
                        p.Email = model.Email;
                        p.Phone = model.Phone;
                        p.FirstName = model.FirstName;
                        p.LastName = model.LastName;
                        p.UserId = model.UserId;
                    }
                    await _personService.UpsertPersonAsync(p, true);
                    return RedirectToAction("Index", "Home", new { area = "Employee" });
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
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
