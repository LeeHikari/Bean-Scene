using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ReservationProject.Data;
using ReservationProject.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReservationProject
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
            services.AddControllers().AddNewtonsoftJson(options =>
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
   );

            services.AddAutoMapper(cfg =>
            {
                cfg.CreateMap<Models.Home.Index, Data.Reservation>();
                cfg.CreateMap<Areas.Admin.Models.Reservation.Create, Data.Reservation>();
                cfg.CreateMap<Areas.Admin.Models.Reservation.Update, Data.Reservation>();
                cfg.CreateMap<Areas.Admin.Models.Reservation.Update, Data.Reservation>().ReverseMap();
                cfg.CreateMap<Areas.Admin.Models.Sitting.Create, Data.Sitting>();
                cfg.CreateMap<Areas.Admin.Models.Sitting.Update, Data.Sitting>().ReverseMap();

            });

            services.AddScoped<PersonService>();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDatabaseDeveloperPageExceptionFilter();

            //services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false) //Set Sign in to require real accounts
            //TODO Change to true to require confirm
            services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddControllersWithViews();

            services.AddRazorPages();
            services.AddCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseMiddleware<StackifyMiddleware.RequestTracerMiddleware>();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseCors(x =>
            {
                x.AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(o=>true)
                .AllowCredentials();

            });

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "myArea",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

            createRolesAsync(serviceProvider);

        }



        private void createRolesAsync(IServiceProvider serviceProvider)
        {

            var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            string[] roleNames = { "Member", "Staff", "Admin","Deactivate" };
            foreach (string roleName in roleNames)
            {
                Task<bool> roleExists = roleManager.RoleExistsAsync(roleName);
                roleExists.Wait();
                if (!roleExists.Result)
                {
                    Task<IdentityResult> result = roleManager.CreateAsync(new IdentityRole(roleName));
                    result.Wait();
                }
            }

            Task<ApplicationUser> findUserTask = UserManager.FindByEmailAsync("admin@bs.com");
            findUserTask.Wait();

            var user = findUserTask.Result;

            // check if the user exists
            if (user == null)
            {
                //Here you could create the super admin who will maintain the web app
                var adminAcc = new ApplicationUser
                {
                    UserName = "admin@bs.com",
                    Email = "admin@bs.com",
                    FirstName = "Admin",
                    LastName = "bs",
                    PhoneNumber = "1234"
                };
                string adminPassword = "Beanscene1!";



                Task<IdentityResult> createadminAcc = UserManager.CreateAsync(adminAcc, adminPassword);
                createadminAcc.Wait();

                var result = createadminAcc.Result;

                if (result.Succeeded)
                {
                    //here we tie the new user to the role
                    Task<IdentityResult> addRoleResult = UserManager.AddToRoleAsync(adminAcc, "Admin");
                    addRoleResult.Wait();

                }
            }

        }

       

    }
} 

            
        



    

