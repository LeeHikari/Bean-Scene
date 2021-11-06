using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReservationProject.Data;
using ReservationProject.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReservationProject.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Roles = "Admin")]
    public class EmployeeController : AdminAreaBaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly PersonService _personService;
        private IMapper _mapper;


        public EmployeeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, PersonService personService, IMapper mapper)
            : base(context)
        {
            _userManager = userManager;
            _personService = personService;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            //TODO Only display employees inside employee index

      
            var model = new Models.Employee.Index
            {
                Staff = await _userManager.GetUsersInRoleAsync("Staff"),
                Admin = await _userManager.GetUsersInRoleAsync("Admin")
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var rolelist = await _context.Roles.Select(s => new
            {
                Value = s.Name,
                Display = s.Name
            }).ToArrayAsync();

            var model = new Models.Employee.Create

            {
                Roles = new SelectList(rolelist.Where(x => x.Display != "Member").ToList(), "Value", "Display")
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Models.Employee.Create model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = $"{model.FirstName.ToLower()}.{model.LastName.ToLower()}@beanscene.com", Email = $"{model.FirstName.ToLower()}.{model.LastName.ToLower()}@beanscene.com", PhoneNumber = model.Phone, FirstName = model.FirstName, LastName = model.LastName };
                var result = await _userManager.CreateAsync(user, model.Password);


                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, model.Role);
                    //No Longer Needed
                    //var p = new Person();
                    //{
                    //    p.Email = $"{model.FirstName.ToLower()}.{model.LastName.ToLower()}@beanscene.com";
                    //    p.Phone = model.Phone;
                    //    p.FirstName = model.FirstName;
                    //    p.LastName = model.LastName;
                    //    p.UserId = user.Id;
                    //}
                    //await _personService.UpsertPersonAsync(p, true);
                    return RedirectToAction("Index", "Home", new { area = "Employee" });
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            model.Roles = new SelectList(await _context.Roles.ToArrayAsync(), "Id", "Name");


            return View(model);
        }

        public async Task<IActionResult> Details(string id)
        {
            try
            {
                if (id == null)
                {
                    return StatusCode(400, "Id Required");
                }

                var person = await _context.Users.FirstOrDefaultAsync(p => p.Id == id);

                if (person == null)
                {
                    return NotFound();
                }
                return View(person);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }


        }
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                if (id == null)
                {
                    return StatusCode(400, "Id Required");
                }

                var person = await _context.Users.FirstOrDefaultAsync(p => p.Id == id);

                if (person == null)
                {
                    return NotFound();
                }
                return View(person);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {
            {
                try
                {
                    if (id == null)
                    {
                        return StatusCode(400, "Id Required");
                    }

                    var person = await _context.Users.FirstOrDefaultAsync(p => p.Id == id);

                    if (person == null)
                    {
                        return NotFound();
                    }
                    return View(person);
                }
                catch (Exception)
                {
                    return StatusCode(500);
                }
            }

            //var rolelist = await _context.Roles.Select(s => new
            //{
            //    Value = s.Name,
            //    Display = s.Name
            //}).ToArrayAsync();
            
            //model.Roles = new SelectList(rolelist.Where(x => x.Display != "Member").ToList(), "Value", "Display");

            //return View(model);
        }


        //TODO Finish Update POST and GET
        [HttpPost]
        public async Task<IActionResult> Update(string id, Models.Employee.Update model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var employee = _mapper.Map<Data.Person>(model);
                    _context.Update<Person>(employee);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    //Exception
                }
            }

            var rolelist = await _context.Roles.Select(s => new
            {
                Value = s.Name,
                Display = s.Name
            }).ToArrayAsync();

            model.Roles = new SelectList(rolelist.Where(x => x.Display != "Member").ToList(), "Value", "Display");

            return View(model);
        }
    }
}
