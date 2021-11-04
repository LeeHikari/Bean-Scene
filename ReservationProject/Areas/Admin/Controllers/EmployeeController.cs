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
        private readonly UserManager<IdentityUser> _userManager;
        private readonly PersonService _personService;
        private IMapper _mapper;


        public EmployeeController(ApplicationDbContext context, UserManager<IdentityUser> userManager, PersonService personService, IMapper mapper)
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

            var s = await _userManager.GetUsersInRoleAsync("Staff");
            var m = await _userManager.GetUsersInRoleAsync("Admin");
            var staffList = new List<Person>();
            var managerList = new List<Person>();

            foreach (var item in s)
            {
                var sList = await _context.People.Where(p => p.UserId == item.Id).FirstOrDefaultAsync();
                if (sList!=null)
                {
                    staffList.Add(sList);

                }
            }
            foreach (var item in m)
            {
                var mList = await _context.People.Where(p => p.UserId == item.Id).FirstOrDefaultAsync();
                if (mList!=null)
                {
                    managerList.Add(mList);
                }
            }
            var model = new Models.Employee.Index
            {
                Staff = staffList,
                Admin=managerList,
            };
          
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var rolelist= await _context.Roles.Select(s=>new
            { 
            Value=s.Name,
            Display=s.Name
            }).ToArrayAsync();

            var model = new Models.Employee.Create

            {
                Roles = new SelectList(rolelist.Where(x=>x.Display != "Member").ToList(), "Value", "Display")
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Models.Employee.Create model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = $"{model.FirstName.ToLower()}.{model.LastName.ToLower()}@beanscene.com", Email = $"{model.FirstName.ToLower()}.{model.LastName.ToLower()}@beanscene.com", PhoneNumber = model.Phone };
                var result = await _userManager.CreateAsync(user, model.Password);
         

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, model.Role);

                    var p = new Person();
                    {
                        p.Email = $"{model.FirstName.ToLower()}.{model.LastName.ToLower()}@beanscene.com";
                        p.Phone = model.Phone;
                        p.FirstName = model.FirstName;
                        p.LastName = model.LastName;
                        p.UserId = user.Id;
                    }
                    await _personService.UpsertPersonAsync(p, true);
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

        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (!id.HasValue)
                {
                    return StatusCode(400, "Id Required");
                }

                var person = await _context.People.FirstOrDefaultAsync(p => p.Id == id.Value);

                if(person == null)
                {
                    return NotFound();
                }
                return View(person);
            }
            catch(Exception)
            {
                return StatusCode(500);
            }
            
            
        }

        public IActionResult Delete()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound();
            }

            var employee = await _context.People.FirstOrDefaultAsync(p => p.Id == id.Value);

            if(employee == null)
            {
                return NotFound();
            }

            var model = _mapper.Map<Models.Employee.Update>(employee);
           
            var rolelist = await _context.Roles.Select(s => new
            {
                Value = s.Name,
                Display = s.Name
            }).ToArrayAsync();
            
            model.Roles = new SelectList(rolelist.Where(x => x.Display != "Member").ToList(), "Value", "Display");

            return View(model);
        }


        //TODO Finish Update POST and GET
        [HttpPost]
        public async Task<IActionResult> Update(int id, Models.Employee.Update model)
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
