using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using HelpDesk.DataAccess.Data;
using ITHelpDesk.DataAccess.Repository.IRepository;
using ITHelpDesk.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ITHelpDesk.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ApplicationDbContext _db;
        private readonly IUnitOfWork _unitOfwork;

        public IndexModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ApplicationDbContext db,
            IUnitOfWork unitOfwork)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _db = db;
            _unitOfwork = unitOfwork; 
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
            public string Department { get; set; }
            public string Position { get; set; }
            public string Picture { get; set; }
        }

        private async Task LoadAsync(IdentityUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var department = _db.User.FirstOrDefault(u => u.Id == user.Id).Department;
            var position = _db.User.FirstOrDefault(u => u.Id == user.Id).Position;

            Username = userName;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                Department = department,
                Position = position
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

            if (!ModelState.IsValid)
            {
                
                await LoadAsync(user);
                return Page();
            }
            var user_ = new Users
            {
                
                Department = Input.Department,

                Position = Input.Position,

                PhoneNumber = Input.PhoneNumber
            };

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }
            
            var objFromDb = _db.User.FirstOrDefault(u => u.Id == user.Id);
            var department = objFromDb.Department;
            var position = objFromDb.Position;

            if (Input.Department != department)
            {
                objFromDb.Department = Input.Department;
                _unitOfwork.Save();
                
            }

            if (Input.Position!= position)
            {
                objFromDb.Position = Input.Position;
                _unitOfwork.Save();

            }



            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Сизнинг профилингиз янгиланди";
            return RedirectToPage();
        }
    }
}
