using Alloggio_MVC.Areas.Manage.Helpers.LayoutService;
using Alloggio_MVC.Helpers.EmailSender;
using Alloggio_MVC.Helpers.LayoutService;
using Core_Layer.Abstract;
using Core_Layer.Entities;
using Data_Layer.Concrete;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Service_Layer.Concrete;
using Service_Layer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alloggio_MVC
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
            services.AddDbContext<DataContext>(opt =>
            opt.UseSqlServer(Configuration.GetConnectionString("Default"))
            );
            services.AddIdentity<AppUser, IdentityRole>(c =>
                {
                    c.Password.RequireDigit = true;
                    c.Password.RequireNonAlphanumeric = false;
                    c.Password.RequiredLength = 6;
                    c.Password.RequireUppercase = true;
                    c.Password.RequireLowercase = true;
                    c.User.RequireUniqueEmail = true;
                    c.SignIn.RequireConfirmedEmail = true;
                }).AddDefaultTokenProviders().AddEntityFrameworkStores<DataContext>();
            services.AddScoped<LayoutService>();
            services.AddScoped<AdminLayoutService>();
            services.AddScoped<SliderRepository>();
            services.AddScoped<RoomRepository>();
            services.AddScoped<BedCountRepository>();
            services.AddScoped<AmenitiesRepository>();
            services.AddScoped<OrderRepository>();
            services.AddScoped<OrderRoomRepository>();
            services.AddScoped<RoomAmenitiesRepository>();
            services.AddScoped<MessageRepository>();
            services.AddScoped<CookingRepository>();
            services.AddScoped<BlogRepository>();
            services.AddScoped<SettingRepository>();

            services.AddScoped<IEmailSender, GmailSender>(x =>
             new GmailSender(
                 Configuration["GmailSender:Host"],
                 Configuration.GetValue<int>("GmailSender:Port"),
                 Configuration.GetValue<bool>("GmailSender:EnableSSL"),
                 Configuration["GmailSender:Username"],
                 Configuration["GmailSender:Password"])
            );
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"
                    );

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
