using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ReservationProject.Data;

namespace ReservationProject.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ApplicationDbContext _context;


        public IndexModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ApplicationDbContext context
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        public string Email { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            

            [Display(Name = "First Name")]
            public string FirstName { get; set; }
            
            [Display(Name = "Last Name")]
            public string LastName { get; set; }
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
        }

        private async Task LoadAsync(IdentityUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            //var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            var userdata = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

            var person = await _context.People.FirstOrDefaultAsync(p => p.UserId == user.Id);

            //Edit Code, Instead of repeatedly calling the database, we're calling it above in person
            //and using the person object to assign the values below.

       


            Email = userName;

            Input = new InputModel
            {
                FirstName = person.FirstName,
                LastName=person.LastName,
                PhoneNumber = person.Phone

            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            var userdata = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
            var person = await _context.People.FirstOrDefaultAsync(p => p.UserId == user.Id);

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }
            //TO DO Edit to change person in database
            if (person.Phone!=Input.PhoneNumber)
            {
                person.Phone = Input.PhoneNumber;
            }
            if (person.FirstName != Input.FirstName)
            {
                person.FirstName = Input.FirstName;

            }
            if (person.LastName != Input.LastName)
            {
                person.LastName = Input.LastName;

            }
            _context.SaveChanges();

            //var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            //if (Input.PhoneNumber != phoneNumber)
            //{
            //    var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
            //    if (!setPhoneResult.Succeeded)
            //    {
            //        StatusMessage = "Unexpected error when trying to set phone number.";
            //        return RedirectToPage();
            //    }
            //}


            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
