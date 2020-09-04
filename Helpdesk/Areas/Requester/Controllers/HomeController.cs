using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using HelpDesk.Models.ViewModels;
using ITHelpDesk.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using ITHelpDesk.Utility;

namespace HelpDesk.Areas.Requester.Controllers
{
    [Area("Requester")]
    [Authorize]
    public class HomeController : Controller
    {
        //Login login = new Login();
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        public HomeController(ILogger<HomeController> logger, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        public IActionResult Index()

        {
            ClaimsPrincipal currentUser = User;           
            
            if (currentUser.IsInRole(SD.Role_Requester))
            {

                return RedirectToAction("Index", "RequestSSender", new { Area = "Requester" });
            }
            if (currentUser.IsInRole(SD.Role_Manager))
            {

                return RedirectToAction("Index", "RequestManager", new { Area = "Manager" });
            }
            if (currentUser.IsInRole(SD.Role_Admin))
            {

                return RedirectToAction("Index", "Users", new { Area = "Admin" });
            }
            if (currentUser.IsInRole(SD.Role_Employee))
            {

                return RedirectToAction("Index", "Requests", new { Area = "Employee" });
            }
            if (currentUser.IsInRole(SD.Role_HeadManager))
            {

                return RedirectToAction("Index", "Requests", new { Area = "HeadManager" });
            }
            return View();
            //return RedirectToAction("Index", "RequestSSender", new { Area = "Requester" });
        }

       
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
