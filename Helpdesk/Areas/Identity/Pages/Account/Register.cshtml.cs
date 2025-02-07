using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using HelpDesk.DataAccess.Data;
using ITHelpDesk.DataAccess.Repository.IRepository;
using ITHelpDesk.Models;
using ITHelpDesk.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ITHelpDesk.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUnitOfWork _unitOfwork;
        // private readonly WorkerManager<Workers> workerManager;

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            RoleManager<IdentityRole> roleManager,
            IUnitOfWork unitOfwork,
            ApplicationDbContext db)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _roleManager = roleManager;
            _unitOfwork = unitOfwork;
            _db = db;
        }
        [BindProperty]
        public string Message1 { get; set; }
        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [RegularExpression("^[a-zA-Z0-9]+[.]?[a-z0-9]{3,20}@ung.uz$", ErrorMessage = "Электрон почта нотўғри кўрсатилган!")]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(15, ErrorMessage = "{0} камида {2} ва кўпи билан {1} хонадан ташкил топиши керак!", MinimumLength = 6)]
            [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{6,15}$", ErrorMessage = "Парол камида битта бош ҳарф, кичик ҳарф, рақам ва символдан ташкил топиши керак!")]
            [DataType(DataType.Password)]
            [Display(Name = "Парол")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "Парол тасдиқланмади, қайтадан киритинг!")]
            public string ConfirmPassword { get; set; }

            [Required]
            [RegularExpression("^[9,7,3][0-9]{8}$", ErrorMessage = "Тел. рақами нотўғри кўрсатилган!")]
            [DataType(DataType.PhoneNumber)]
            [Display(Name = "PhoneNumber")]
            public string PhoneNumber { get; set; }







            [Required]
            public string FName { get; set; }

            [Required]
            public string Department { get; set; }

            [Required]
            public string Position { get; set; }

            [Required]
            public string LName { get; set; }

            [Required]
            public int AddressId { get; set; }


            public string Role { get; set; }
            public string ManagerId { get; set; }

            public IEnumerable<SelectListItem> BuildingList { get; set; }
            public IEnumerable<SelectListItem> BlockList { get; set; }
            public IEnumerable<SelectListItem> RoleList { get; set; }
            public IEnumerable<SelectListItem> ManagerList { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            var userList = _db.User.ToList();
            var userRole = _db.UserRoles.ToList();
            var roles = _db.Roles.ToList();
            foreach (var user in userList)
            {
                var roleId = userRole.FirstOrDefault(u => u.UserId == user.Id).RoleId;
                user.Role = roles.FirstOrDefault(u => u.Id == roleId).Name;

            }


            Input = new InputModel()
            {
                BuildingList = _unitOfwork.Address.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Full,
                    Value = i.AddressId.ToString()
                }),
                RoleList = _roleManager.Roles.Select(x => x.Name).Select(i => new SelectListItem
                {
                    Text = i,
                    Value = i,
                    Selected = false
                }),

                ManagerList = userList.Where(u => u.Role == "Manager").Select(i => new SelectListItem
                {
                    Text = i.Fullname,
                    Value = i.Id.ToString()
                }),


            };


            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            var userList = _db.User.ToList();
            returnUrl = returnUrl ?? Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            foreach(var item in userList) {
                if (item.Email == Input.Email)
                {
                    Message1 = "Электрон почта мавжуд";
                }
            }
            if (ModelState.IsValid)
            {

                var user = new Users
                {
                    UserName = Input.Email,
                    Email = Input.Email,
                    AddressId = Input.AddressId,
                    FName = Input.FName,
                    LName = Input.LName,
                    PhoneNumber = Input.PhoneNumber,
                    Role = Input.Role,
                    Department = Input.Department,
                    Position = Input.Position,
                    EmailConfirmed = true
                };

                var manager = new Manager
                {
                    UserId = user.Id
                };

                var requester = new RequestMakers
                {
                    UserId = user.Id
                };
                var worker2 = new Workers
                {
                    UserId = user.Id
                };
                var worker = new Workers();
                if (user.Role != null && user.Role == SD.Role_Employee)
                {
                    var managers = _db.Managers.ToList();
                    var managerId = managers.FirstOrDefault(u => u.UserId == Input.ManagerId).ManagerId;
                    worker.UserId = user.Id;
                    worker.ManagerId = managerId;
                    worker.State = "working";

                }

                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                    var comfirmationLink = Url.Action("ConfirmEmail", "Account",
                        new { userId = user.Id, token }, Request.Scheme);

                    _logger.Log(LogLevel.Warning, comfirmationLink);

                    _logger.LogInformation("User created a new account with password.");

                    if (!await _roleManager.RoleExistsAsync(SD.Role_Admin))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin));
                    }
                    if (!await _roleManager.RoleExistsAsync(SD.Role_HeadManager))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(SD.Role_HeadManager));
                    }
                    if (!await _roleManager.RoleExistsAsync(SD.Role_Manager))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(SD.Role_Manager));

                    }
                    if (!await _roleManager.RoleExistsAsync(SD.Role_Employee))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(SD.Role_Employee));
                    }
                    if (!await _roleManager.RoleExistsAsync(SD.Role_Requester))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(SD.Role_Requester));
                    }


                    if (user.Role == null)
                    {
                        //await _userManager.AddToRoleAsync(user, SD.Role_Requester);
                        user.Role = SD.Role_Requester;
                    }

                    else
                    {
                        if (user.Role == SD.Role_Requester)
                        {
                            _unitOfwork.Requester.Add(requester);

                        }
                        if (user.Role == SD.Role_Manager)
                        {
                            _unitOfwork.Manager.Add(manager);
                        }
                        if (user.Role == SD.Role_Employee)
                        {
                            _unitOfwork.Worker.Add(worker);
                        }
                        await _userManager.AddToRoleAsync(user, user.Role);

                    }


                    if (user.Role == SD.Role_Requester)
                    {
                        _unitOfwork.Requester.Add(requester);
                        //Console.WriteLine(requester.RequestmakerId + " and " + requester.UserId);
                        await _userManager.AddToRoleAsync(user, user.Role);
                        user.Role = null;
                    }
                    //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    //code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    //var callbackUrl = Url.Page(
                    //    "/Account/ConfirmEmail",
                    //    pageHandler: null,
                    //    values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                    //    protocol: Request.Scheme);

                    //await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                    //    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        if (user.Role == null)
                        {
                            await _signInManager.SignInAsync(user, isPersistent: false);
                            return LocalRedirect(returnUrl);
                        }
                        else
                        {
                            //admin is registering new user
                            return RedirectToAction("Index", "Users", new { Area = "Admin" });
                        }
                    }
                    //ViewBag.ErrorTitle = "Registration is successfull";

                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            Input = new InputModel()
            {
                BuildingList = _unitOfwork.Address.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Full,
                    Value = i.AddressId.ToString()
                }),
                RoleList = _roleManager.Roles.Select(x => x.Name).Select(i => new SelectListItem
                {
                    Text = i,
                    Value = i,
                    Selected = false
                }),

                ManagerList = userList.Where(u => u.Role == "Manager").Select(i => new SelectListItem
                {
                    Text = i.Fullname,
                    Value = i.Id.ToString()
                }),
            };
            return Page();
        }
    }
}
