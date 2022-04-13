using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Service.DataLayer.Template.ViewModels;
using Service.DataLayer.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Service.DataLayer;
using System.Reflection;
using System.Runtime.CompilerServices;
using Service.DataLayer.Models;
using Microsoft.Extensions.Configuration;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Dynamic;

namespace Template.Controllers
{
    //Audit when save add this await _context.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
    [Authorize(Roles = AppConstants.Role.Names.Admin_Role_Name)]
   //[AllowAnonymous]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly AppDBContext _context;
        private readonly IConfiguration config;

        public AdminController(UserManager<ApplicationUser> userManager,
           SignInManager<ApplicationUser> signInManager,
           RoleManager<IdentityRole> roleManager,
           AppDBContext context,
           IConfiguration config)

        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            _context = context;
            this.config = config;
        }

        #region Account Section
        /**************Account Creation Section******************************************/
        //[AllowAnonymous]
        public IActionResult Dashboard()
        {
            var countAccount = userManager.Users.Count();
            var countRoles = roleManager.Roles.Count();

            var viewModel = new DashboardViewModel();

            viewModel.CountAccount = countAccount;
            viewModel.CountRoles = countRoles;

            return View(viewModel);
        }

        [HttpGet]
        //[Authorize(Roles = "Admin")]
        [AllowAnonymous]
        //[Authorize]

        public IActionResult AddNewAccount()
        {
            AccountViewModel model = GetRoles();

            return View(model);
        }

        [HttpPost]
        //[Authorize]
        // [AllowAnonymous]
        public async Task<IActionResult> AddNewAccount(AccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Username,
                    Email = model.Username,
                    CreatedDateTime = DateTime.Now,
                    //CreatedBy = ;
                };

                // Store user data in AspNetUsers database table
                var result = await userManager.CreateAsync(user, model.Password);

                // If user is successfully created, sign-in the user using
                // SignInManager and redirect to index action of HomeController
                if (result.Succeeded)
                {
                    //await signInManager.SignInAsync(user, isPersistent: false);
                    //return RedirectToAction("ListRoles", "Admin");

                    if (signInManager.IsSignedIn(User) && User.IsInRole(AppConstants.Role.Names.Admin_Role_Name))
                    {
                        return RedirectToAction("ListUsers", "Admin");
                    }

                    await signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("ListUsers");
                }

                // If there are any errors, add them to the ModelState object
                // which will be displayed by the validation summary tag helper
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            model = GetRoles();
            return View(model);
        }

        [AcceptVerbs("Get", "Post")]
        //[Authorize]
        //[AllowAnonymous]
        public async Task<IActionResult> IsEmailInUse(string email)
        {
            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return Json(true);
            }
            else
            {
                return Json($"Email {email} is already in use.");
            }
        }

        /**************End of Account Creation Section******************************************/
        #endregion

        #region Role Section
        /**************Role Section******************************************/

        [HttpGet]
        [Authorize("ListRoles-AdminController")]
        public IActionResult ListRoles()
        {
            var roles = roleManager.Roles;
            return View(roles);
        }

        //[AllowAnonymous]
        [HttpGet]
        public IActionResult CreateRole()
        {
            //CreateRoleViewModel model = GetRoles();
            return View();
        }

        //[AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                // We just need to specify a unique role name to create a new role
                IdentityRole identityRole = new IdentityRole
                {
                    Name = model.RoleName
                };

                // Saves the role in the underlying AspNetRoles table
                IdentityResult result = await roleManager.CreateAsync(identityRole);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles");
                }

                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }

        private AccountViewModel GetRoles()
        {
            AccountViewModel model = new AccountViewModel();
            model.Roles = roleManager.Roles.ToList();
            return model;
        }

        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
            // Find the role by Role ID
            var role = await roleManager.FindByIdAsync(id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {id} cannot be found";
                return View("NotFound");
            }

            var model = new EditRoleViewModel
            {
                Id = role.Id,
                RoleName = role.Name
            };

            // Retrieve all the Users
            foreach (var user in userManager.Users)
            {
                // If the user is in this role, add the username to
                // Users property of EditRoleViewModel. This model
                // object is then passed to the view for display
                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    model.Users.Add(user.UserName);
                }
            }

            return View(model);
        }

        // This action responds to HttpPost and receives EditRoleViewModel
        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleViewModel model)
        {
            var role = await roleManager.FindByIdAsync(model.Id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {model.Id} cannot be found";
                return View("NotFound");
            }
            else
            {
                role.Name = model.RoleName;

                // Update the Role using UpdateAsync
                var result = await roleManager.UpdateAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await roleManager.FindByIdAsync(id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {id} cannot be found";
                return View("NotFound");
            }
            else
            {
                var result = await roleManager.DeleteAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View("ListRoles");
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditUsersInRole(string roleId)
        {
            ViewBag.roleId = roleId;

            var role = await roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found";
                return View("NotFound");
            }

            var model = new List<UserRoleViewModel>();

            foreach (var user in userManager.Users)
            {
                var userRoleViewModel = new UserRoleViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };

                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    userRoleViewModel.IsSelected = true;
                }
                else
                {
                    userRoleViewModel.IsSelected = false;
                }

                model.Add(userRoleViewModel);
            }

            return PartialView("_EditUsersInRolePartial", model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUsersInRole(List<UserRoleViewModel> model, string roleId)
        {
            var role = await roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found";
                return View("NotFound");
            }

            for (int i = 0; i < model.Count; i++)
            {
                var user = await userManager.FindByIdAsync(model[i].UserId);

                IdentityResult result = null;

                if (model[i].IsSelected && !(await userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await userManager.AddToRoleAsync(user, role.Name);
                }
                else if (!model[i].IsSelected && await userManager.IsInRoleAsync(user, role.Name))
                {
                    result = await userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }

                if (result.Succeeded)
                {
                    if (i < (model.Count - 1))
                        continue;
                    else
                        return RedirectToAction("EditRole", new { Id = roleId });
                }
            }

            return RedirectToAction("EditRole", new { Id = roleId });
        }


        /**************End Role Section******************************************/
        #endregion

        #region User Section
        /**************User Section******************************************/
        [HttpGet]
        //[Authorize("ListUsers-AdminController")]
        public IActionResult ListUsers()
        {
            var users = userManager.Users;
            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                return View("NotFound");
            }

            // GetClaimsAsync retunrs the list of user Claims
            var userClaims = await userManager.GetClaimsAsync(user);
            // GetRolesAsync returns the list of user Roles
            var userRoles = await userManager.GetRolesAsync(user);

            var model = new EditUserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                //City = user.City,
                Claims = userClaims.Select(c => c.Value).ToList(),
                Roles = userRoles
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            var user = await userManager.FindByIdAsync(model.Id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {model.Id} cannot be found";
                return View("NotFound");
            }
            else
            {
                user.Email = model.Email;
                user.UserName = model.UserName;
                //user.City = model.City;

                var result = await userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListUsers");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                return View("NotFound");
            }
            else
            {
                var result = await userManager.DeleteAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListUsers");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View("ListUsers");
            }
        }

        [HttpGet]
        public async Task<IActionResult> ManageUserRoles(string Id)
        {
            ViewBag.userId = Id;

            var user = await userManager.FindByIdAsync(Id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {Id} cannot be found";
                return View("NotFound");
            }

            var model = new List<UserRolesViewModel>();

            foreach (var role in roleManager.Roles)
            {
                var userRolesViewModel = new UserRolesViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name
                };

                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    userRolesViewModel.IsSelected = true;
                }
                else
                {
                    userRolesViewModel.IsSelected = false;
                }

                model.Add(userRolesViewModel);
            }

            return PartialView("_ManageUserRolesPartial", model);
            //return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ManageUserRoles(List<UserRolesViewModel> model, string userId)
        {
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                return View("NotFound");
            }

            var roles = await userManager.GetRolesAsync(user);
            var result = await userManager.RemoveFromRolesAsync(user, roles);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove user existing roles");
                return View(model);
            }

            result = await userManager.AddToRolesAsync(user,
                model.Where(x => x.IsSelected).Select(y => y.RoleName));

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot add selected roles to user");
                return View(model);
            }

            return RedirectToAction("EditUser", "Admin", new { Id = userId });
        }


        [HttpGet]
        public async Task<IActionResult> ManageUserClaims(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                return View("NotFound");
            }

            // UserManager service GetClaimsAsync method gets all the current claims of the user
            var existingUserClaims = await userManager.GetClaimsAsync(user);

            var model = new UserClaimsViewModel
            {
                UserId = userId
            };

            // Loop through each claim we have in our application
            //foreach (Claim claim in ClaimsStore.AllClaims)
            foreach (Claim claim in GetAllControllerActionsUpdated())
            {
                UserClaim userClaim = new UserClaim
                {
                    ClaimType = claim.Type
                };

                // If the user has the claim, set IsSelected property to true, so the checkbox
                // next to the claim is checked on the UI
                if (existingUserClaims.Any(c => c.Type == claim.Type))
                {
                    userClaim.IsSelected = true;
                }

                model.Cliams.Add(userClaim);
            }

            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> ManageUserClaims(UserClaimsViewModel model)
        {
            var user = await userManager.FindByIdAsync(model.UserId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {model.UserId} cannot be found";
                return View("NotFound");
            }

            // Get all the user existing claims and delete them
            var claims = await userManager.GetClaimsAsync(user);
            var result = await userManager.RemoveClaimsAsync(user, claims);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove user existing claims");
                return View(model);
            }

            // Add all the claims that are selected on the UI
            result = await userManager.AddClaimsAsync(user,
                model.Cliams.Where(c => c.IsSelected).Select(c => new Claim(c.ClaimType, c.ClaimType)));

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot add selected claims to user");
                return View(model);
            }

            return RedirectToAction("EditUser", new { Id = model.UserId });

        }

        /**************End User Section******************************************/
        #endregion

        #region Get All Controller Action
        public List<Claim> GetAllControllerActionsUpdated()
        {
            Assembly asm = Assembly.GetExecutingAssembly();

            var controlleractionlist = asm.GetTypes()
                .Where(type => typeof(Controller).IsAssignableFrom(type))
                .SelectMany(type =>
                    type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
                .Where(m => !m.GetCustomAttributes(typeof(CompilerGeneratedAttribute),
                    true).Any())
                .Select(x => new
                {
                    Controller = x.DeclaringType.Name,
                    Action = x.Name,
                    ReturnType = x.ReturnType.Name,
                    Attributes = string.Join(",",
                        x.GetCustomAttributes().Select(a => a.GetType().Name.Replace("Attribute", "")))
                })
                .OrderBy(x => x.Controller).ThenBy(x => x.Action).ToList();

            var AllClaims = new List<Claim>();


            foreach (var item in controlleractionlist)
            {
                var ClaimName = item.Action + "-" + item.Controller;

                var claim = new Claim(ClaimName, ClaimName);
                AllClaims.Add(claim);
            }
            return AllClaims;
        }
        #endregion

        #region Application Setting
        [Authorize("AppSetting-AdminController")]
        public IActionResult AppSetting()
        {
           var model = FillAppSetting();

            return View(model);
        }
        [HttpPost]
        public IActionResult AppSetting(Setting model)
        {
            if (ModelState.IsValid)
            {
                UpdateSetting(model);

                ViewBag.success = "تم حفظ الاعدادات بنجاح";
                return View(model);
            }

            ModelState.AddModelError("", "يجب ادخال جميع الاعدادات");
            return View(model);
        }

        private static void UpdateSetting(Setting model)
        {
            var appSettingsPath = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "appsettings.json");
            var json = System.IO.File.ReadAllText(appSettingsPath);

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.Converters.Add(new ExpandoObjectConverter());
            jsonSettings.Converters.Add(new StringEnumConverter());

            dynamic config = JsonConvert.DeserializeObject<ExpandoObject>(json, jsonSettings);

            config.AppSetting.AccessFailedCount = model.AccessFailedCount;
            config.AppSetting.UserPasswordLength = model.UserPasswordLength;
            config.AppSetting.PassRequireDigit = model.PassRequireDigit;
            config.AppSetting.PassRequireLowercase = model.PassRequireLowercase;
            config.AppSetting.AccessFailedCount = model.AccessFailedCount;
            config.AppSetting.PassRequireUppercase = model.PassRequireUppercase;
            config.AppSetting.PassRequireNonAlphanumeric = model.PassRequireNonAlphanumeric;
            config.AppSetting.UserSessionTimeOut = model.UserSessionTimeOut;
            config.AppSetting.MaxLimitToLockUser = model.MaxLimitToLockUser;
            config.AppSetting.UserConfirmPolicyTitle = model.UserConfirmPolicyTitle;
            config.AppSetting.UserConfirmPolicy = model.UserConfirmPolicy;
            config.AppSetting.ApplicationName = model.ApplicationName;
            config.AppSetting.ApplicationLogo = model.ApplicationLogo;
            config.AppSetting.EmpDataApiUrl = model.EmpDataApiUrl;
            config.AppSetting.EnableConfirmPolicy = model.EnableConfirmPolicy;
            config.AppSetting.EnableRightClick = model.EnableRightClick;

            var newJson = JsonConvert.SerializeObject(config, Formatting.Indented, jsonSettings);
            System.IO.File.WriteAllText(appSettingsPath, newJson);
        }

        private Setting FillAppSetting()
        {
            try
            {
                Setting model = new Setting();
                model.AccessFailedCount = config.GetValue<int>("AppSetting:AccessFailedCount");
                model.UserPasswordLength = config.GetValue<int>("AppSetting:UserPasswordLength");
                model.PassRequireDigit = config.GetValue<bool>("AppSetting:PassRequireDigit");
                model.PassRequireLowercase = config.GetValue<bool>("AppSetting:PassRequireLowercase");
                model.PassRequireUppercase = config.GetValue<bool>("AppSetting:PassRequireUppercase");
                model.PassRequireNonAlphanumeric = config.GetValue<bool>("AppSetting:PassRequireNonAlphanumeric");
                model.UserSessionTimeOut = config.GetValue<int>("AppSetting:UserSessionTimeOut");
                model.MaxLimitToLockUser = config.GetValue<int>("AppSetting:MaxLimitToLockUser");
                model.UserConfirmPolicyTitle = config.GetValue<string>("AppSetting:UserConfirmPolicyTitle");
                model.UserConfirmPolicy = config.GetValue<string>("AppSetting:UserConfirmPolicy");
                model.ApplicationName = config.GetValue<string>("AppSetting:ApplicationName");
                model.ApplicationLogo = config.GetValue<string>("AppSetting:ApplicationLogo");
                model.EmpDataApiUrl = config.GetValue<string>("AppSetting:EmpDataApiUrl");
                model.EnableConfirmPolicy = config.GetValue<string>("AppSetting:EnableConfirmPolicy");
                model.EnableRightClick = config.GetValue<string>("AppSetting:EnableRightClick");

                return model;
            }
            catch (Exception)
            {

                throw;
            }
            
        }
        #endregion

    }
}
