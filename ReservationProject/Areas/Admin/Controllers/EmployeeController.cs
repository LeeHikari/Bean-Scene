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
                try
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
                        return RedirectToAction(nameof(Index));
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                catch (Exception)
                {
                    StatusCode(500);
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
                var roles = await _userManager.GetRolesAsync(person);

                var employee = new Models.Employee.Update(person);

                employee.Role = roles[0];

                return View(employee);
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
                var roles = await _userManager.GetRolesAsync(person);

                var employee = new Models.Employee.Update(person);

                employee.Role = roles[0];

                return View(employee);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }

        }
        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(string id)
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
                //remove roles

                var roles = await _userManager.GetRolesAsync(person);

                foreach (var role in roles)
                {
                    IdentityResult deletionResult = await _userManager.RemoveFromRoleAsync(person, role);
                }

                await _userManager.DeleteAsync(person);

                return RedirectToAction(nameof(Index));
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
                    var rolelist = await _context.Roles.Select(s => new
                    {
                        Value = s.Name,
                        Display = s.Name
                    }).ToArrayAsync();

                    var person = await _context.Users.FirstOrDefaultAsync(p => p.Id == id);

                    if (person == null)
                    {
                        return NotFound();
                    }
                    var roles = await _userManager.GetRolesAsync(person);
                    
                    var employee = new Models.Employee.Update(person);

                    employee.Role = roles[0];
                    employee.Roles = new SelectList(rolelist.Where(x => x.Display != "Member").ToList(), "Value", "Display");

                    return View(employee);
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
        [ActionName("Update")]
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
                    if (id == null)
                    {
                        return StatusCode(400, "Id Required");
                    }


                    var person = await _context.Users.FirstOrDefaultAsync(p => p.Id == id);

                    if (person == null)
                    {
                        return NotFound();
                    }
                    //remove roles

                    var roles = await _userManager.GetRolesAsync(person);

                    foreach (var role in roles)
                    {
                        IdentityResult deletionResult = await _userManager.RemoveFromRoleAsync(person, role);
                    }


                    person.UserName = $"{model.FirstName.ToLower()}.{model.LastName.ToLower()}@beanscene.com";
                    person.Email = $"{model.FirstName.ToLower()}.{model.LastName.ToLower()}@beanscene.com";
                    person.PhoneNumber = model.Phone;
                    person.FirstName = model.FirstName;
                    person.LastName = model.LastName;
                    
                    //add selected role
                    await _userManager.AddToRoleAsync(person, model.Role);
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
