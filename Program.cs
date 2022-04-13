using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Service.DataLayer.Models;
using Service.DataLayer.Security;
using System.Threading.Tasks;
using Template.Models;

namespace Template
{
    public class Program
    {
        //private static HttpContext _httpContext => new HttpContextAccessor().HttpContext;
        public async static Task Main(string[] args)
        {          
            var host = CreateHostBuilder(args).Build();
            using var scope = host.Services.CreateScope();
            var service = scope.ServiceProvider;
            
            try
            {
                var context = service.GetRequiredService<AppDBContext>();
                var conf = service.GetRequiredService<IConfiguration>();
                var userManager = service.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = service.GetRequiredService<RoleManager<IdentityRole>>();
                //do migrate to database
                await context.Database.MigrateAsync();
                //migrate Admin User
                await Seed.SeedAdminUser(roleManager, userManager, conf);
            }
            catch (System.Exception)
            {
                throw;
            }

           await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {                    
                    webBuilder.UseStartup<Startup>();
                });        
    }
}
