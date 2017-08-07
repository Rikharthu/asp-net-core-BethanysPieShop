using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using BethanysPieShop.Models;
using Microsoft.Extensions.Configuration;
using MySQL.Data.EntityFrameworkCore.Extensions;
using BethanysPieShop.Data;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BethanysPieShop
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true) // like appsettings.production.json
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // add MVC support. This behind the scenes configures a number of other services
            services.AddMvc();

            // register our own services for DI
            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddTransient<IPieRepository, PieRepository>();
            services.AddTransient<IOrderRepository, OrderRepository>();
            // Add session support
            services.AddMemoryCache();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSession();
            services.AddScoped<ShoppingCart>(sp => ShoppingCart.GetCart(sp));

            // Register our Database Contexts and specify connecion string to use
            //string connectionString = Configuration.GetConnectionString("MySqlConnection");
            string connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<AppDbContext>(
                // options action. configure options to use MySQL via specified connection string
                //options => options.UseMySQL(connectionString)
                options => options.UseSqlServer(connectionString)
            );

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>(); // specify IdentityDbContext                  
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, AppDbContext context)
        {
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/BethanyPieShopError");
            }

            app.UseStatusCodePages();

            // allow serving static files (give access to wwwroot/ folder)
            app.UseStaticFiles();

            // Configure the ability to work with sessions (place it before UseMvc())
            app.UseSession();

            app.UseIdentity();

            // {controller=Home}/{action=Index}/{id?}
            //app.UseMvcWithDefaultRoute();
            //app.UseMvc(b =>
            //{
            //    b.MapRoute(
            //        name: "pie_list",
            //        template: "{controller=Pie}/{action=list}");
            //});
            app.UseMvc(routes =>
            {   // first matched route will handle the request => mind the sequence!
                routes.MapRoute(
                    name: "categoryFilter",
                    template: "Pie/{action = Index}/{category?}",
                    defaults: new { Controller = "Pie", action = "List" }
                    );

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}"
                    );
            });

            DbInitializer.Initialize(context);
        }
    }
}
