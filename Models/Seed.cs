using Microsoft.AspNetCore.Identity;
using Service.DataLayer;
using Service.DataLayer.Models;
using Service.DataLayer.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Template.Models
{
    public class Seed
    {
        public static async Task SeedAdminUser(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, Microsoft.Extensions.Configuration.IConfiguration config)
        {
            var username = config["UserRole:Admin"];
            var password = config["UserRole:Admin_Password"];

            var user = new ApplicationUser
            {
                UserName = username,
            };


            if (! roleManager.Roles.Any(a=>a.Name == username))
            {
                await roleManager.CreateAsync(new IdentityRole { Name = username });
            }

            if (!userManager.Users.Any(u => u.UserName == user.UserName))
            {
                await userManager.CreateAsync(user, password);
                await userManager.AddToRoleAsync(user, username);
            }
        }
    }
}
