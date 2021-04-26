using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using OnlineElection.Models;
using System.Threading.Tasks;
using OnlineElection.Services;
using Microsoft.Extensions.Logging;

namespace OnlineElection
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
            services.AddHttpContextAccessor();
            services.AddTransient<JSONService<Dictionary<string, long>>>();
            services.AddControllersWithViews();
            services.AddMvc();
          
            services.AddTransient<IHash,HashSevice>();
            services.AddTransient<ITokenGenerator, TokenGenerator>();
            services.AddTransient<ISendEmailAsync, EmailService>();
         //   services.AddTransient<EmailSendService>();
            services.AddSingleton<IHostedService, ServiceT>();
            var serviceProvider = services.BuildServiceProvider();
            var logger = serviceProvider.GetService<ILogger<ServiceT>>();
            services.AddSingleton(typeof(ILogger), logger);

            string connection = Configuration.GetConnectionString("DefaultConnection");
            //  добавляем контекст MobileContext в качестве сервиса в приложение
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connection), ServiceLifetime.Transient);

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(opt =>

            {
                opt.LoginPath = new Microsoft.AspNetCore.Http.PathString("/User/Login");
                opt.LogoutPath = new Microsoft.AspNetCore.Http.PathString("/User/Logout");
            }
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
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllers();
            }
         );
        }
    }
}
