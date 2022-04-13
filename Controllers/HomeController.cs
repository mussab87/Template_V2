using Template.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Service.DataLayer.Security;
using Service.DataLayer.Models;
using Microsoft.AspNetCore.Http;
using Template.Filters;

namespace Template.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;

        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly AppDBContext _context;

        public HomeController(UserManager<ApplicationUser> userManager,
           SignInManager<ApplicationUser> signInManager,
           RoleManager<IdentityRole> roleManager,
           AppDBContext context
           )
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            _context = context;
        }

        //[AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        //[Authorize]
        //[AllowAnonymous]
        public async Task<IActionResult> Home() 
        {
            //check if first time for login redirect user to change password page before process anything on the system
            var user = await userManager.GetUserAsync(User);
            if (user.FirstLogin == null)
            {
                ViewBag.changePassword = "true";
               return RedirectToAction("ChangePassword", "Account");
            }

            //var getSetting = _Setting.GetAppSetting();
            //if (getSetting == null)
            //    return RedirectToAction("AppSetting", "Admin");
            //SessionHelper.SetObjectAsJson(HttpContext.Session, "settingObject", getSetting);
            //SessionHelper.GetObjectFromJson<Setting>(HttpContext.Session, "settingObject");
            //HttpContext.Session.se("setting", getSetting);


            return View();
        }        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
