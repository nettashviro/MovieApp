using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using MovieApp.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using MovieApp.Models;
using MovieApp.Services;

namespace MovieApp
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
            services.AddControllersWithViews();

            services.AddDbContext<MovieAppContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("MovieAppContext")));

            services.AddSession(options => { options.IdleTimeout = TimeSpan.FromMinutes(1); });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
              .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, (options) =>
              {
                  options.LoginPath = new PathString("/Account/Login/");
                  options.AccessDeniedPath = new PathString("/Errors/AccessDenied/");
              });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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

            app.Use(async (context, next) =>
            {
                await next();
                if (context.Response.StatusCode == 404) // Page Not Found
                {
                    context.Request.Path = "/Errors/PageNotFound";
                    await next();
                }

                if(context.Response.StatusCode ==400 )//Bad request
                {
                    context.Request.Path = "/Errors/BadRequest";
                    await next();
                }
                if (context.Response.StatusCode == 500)//Bad request
                {
                    context.Request.Path = "/Errors/InternalError";
                    await next();
                }


            });


            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseSession();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
