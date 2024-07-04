using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DinningRoom.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DinningRoom
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "menu",
                    pattern: "menu",
                    defaults: new { controller = "Menu", action = "Index" });
                endpoints.MapControllerRoute(
        name: "menu",
        pattern: "menu",
        defaults: new { controller = "Menu", action = "Index" });

                endpoints.MapControllerRoute(
                    name: "menu-create",
                    pattern: "menu/create",
                    defaults: new { controller = "MenuRedaction", action = "Create" });

                endpoints.MapControllerRoute(
                    name: "menu-edit",
                    pattern: "menu/edit/{id}",
                    defaults: new { controller = "MenuRedaction", action = "Edit" });

                endpoints.MapControllerRoute(
                    name: "menu-delete",
                    pattern: "menu/delete/{id}",
                    defaults: new { controller = "MenuRedaction", action = "Delete" });

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                   name: "order",
                   pattern: "menu",
                   defaults: new { controller = "Menu", action = "OrderMultipleItems" });
            });
        }
    }
}