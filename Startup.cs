using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Service.DataLayer.Connection.Configuration;
using Service.DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Service.DataLayer.Security;
using Service.DataLayer;
using Template.Models;
using Template.Filters;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace Template
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            ////string username = "sa";
            ////string password = "123";
            ////string SecretKey = "RSLFEncrypti0nConnect0n@Username&K6yPasswordRSLF";
            ////var testReturnUsername = Crypto.EncryptStringDES(username, SecretKey);
            ////var testReturnPassword = Crypto.EncryptStringDES(password, SecretKey);

            //for decrypt username & password
            //string DatabaseConn = DBAppConfigHelper.ConnDb(
            //     Configuration.GetConnectionString("ServiceDBConnection"),
            //     Configuration.GetConnectionString("DBUsername"),
            //     Configuration.GetConnectionString("DBPassword")
            //     );

            services.AddSingleton<HtmlEncoder>(
                HtmlEncoder.Create(allowedRanges: new[]
                {
                    UnicodeRanges.BasicLatin,
                    UnicodeRanges.Arabic
                }));
            //API link

            // TODO: consider Typed clients: https://docs.microsoft.com/en-us/aspnet/core/fundamentals/http-requests?view=aspnetcore-6.0#typed-clients
            services.AddHttpClient("soldier", c =>
            {
                c.BaseAddress = new Uri(Configuration.GetValue<string>("AppSetting:EmpDataApiUrl"));
            });



            //Configuration.GetConnectionString("ServiceDBConnection")
            services.AddEntityFrameworkSqlServer();

            services.AddDbContext<AppDBContext>(
            options => options.UseSqlServer(Configuration.GetConnectionString("ServiceDBConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>(
                o =>
                {
                    // configure identity options
                    o.Password.RequireDigit = Configuration.GetValue<bool>("AppSetting:PassRequireDigit");
                    o.Password.RequireLowercase = Configuration.GetValue<bool>("AppSetting:PassRequireLowercase");
                    o.Password.RequireUppercase = Configuration.GetValue<bool>("AppSetting:PassRequireUppercase");
                    o.Password.RequireNonAlphanumeric = Configuration.GetValue<bool>("AppSetting:PassRequireNonAlphanumeric");
                    o.Password.RequiredLength = Configuration.GetValue<int>("AppSetting:UserPasswordLength");
                })
                .AddRoles<IdentityRole>()
                .AddRoleManager<RoleManager<IdentityRole>>()
                .AddSignInManager<SignInManager<ApplicationUser>>()
                .AddEntityFrameworkStores<AppDBContext>()
                .AddDefaultTokenProviders();

            //Auto logout on session expire
            services.ConfigureApplicationCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromMinutes(Configuration.GetValue<int>("AppSetting:UserSessionTimeOut"));
                options.LoginPath = "/Account/Login";
                options.SlidingExpiration = true;
            });
            
            services.Configure<SecurityStampValidatorOptions>(options =>
            {
                options.ValidationInterval = TimeSpan.Zero;
            });

            //services.AddMvc(config => {
            //    var policy = new AuthorizationPolicyBuilder()
            //                    .RequireAuthenticatedUser()
            //                    .Build();
            //    config.Filters.Add(new AuthorizeFilter(policy));
            //});

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(60);
            });

            services.AddControllersWithViews(config =>
            {
                var policy = new AuthorizationPolicyBuilder()
                                .RequireAuthenticatedUser()
                                .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            }
            );
            //services.AddScoped<Ih, HttpContextFactory>();
            services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
            services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();            
            //services.AddTransient<ISettingRepository, SettingRepository>();
            //services.AddSingleton<IConfiguration>(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) //, Seed seeder
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseSession();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            //seeder.SeedAdminUser();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
            
        }
    }
}
