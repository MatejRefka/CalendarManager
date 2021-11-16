using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;
using CalendarManagerLibrary;
using Microsoft.AspNetCore.Authentication.Cookies;
using CalendarManager.Services;
using CalendarManager.Data;

namespace CalendarManager {

    public class Startup {

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration) {

            Configuration = configuration;
        }

        //Adding services to the container
        public void ConfigureServices(IServiceCollection services) {

            services.AddControllersWithViews();
            services.AddRazorPages();

            //Adding Google Authentication service
            //source code: https://docs.microsoft.com/en-us/aspnet/core/security/authentication/social/google-logins?view=aspnetcore-5.0
            services.AddAuthentication().AddGoogle(options => {
                IConfigurationSection googleAuthNSection =
                    Configuration.GetSection("Authentication:Google");

                options.ClientId = googleAuthNSection["ClientId"];
                options.ClientSecret = googleAuthNSection["ClientSecret"];
                options.SaveTokens = true;
                options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            });

            //Adding class library classes and services classes
            services.AddSingleton<StateToken>();
            services.AddSingleton<Rounder>();
            services.AddSingleton<ShortURL>();
            services.AddSingleton<EventDeserializer>();
            services.AddSingleton<EventFormatter>();

            services.AddDbContextPool<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("ApplicationDBConnection")));
            /*using (var context = new ApplicationDbContext()) {

                context.Database.EnsureCreated();

                context.SaveChanges();

            }*/





        }

        //Configuring the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else {
                app.UseExceptionHandler("/Home/Error");
                //Default HSTS value = 30 days
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => {

                //custom route to capture user input from browser
                endpoints.MapControllerRoute(name: "bookingAction",
                pattern: "BookingSession/{bookedEvents?}",
                defaults: new { controller = "Calendar", action = "BookingSession" });

                //custom route for the dynamic booking short URL (id)
                endpoints.MapControllerRoute(name: "bookingPage",
                pattern: "BookingSession/{id?}",
                defaults: new { controller = "BookingSession", action = "Index" });

                //default route when no other custom route specified
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}

