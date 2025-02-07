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
using HelpDesk.DataAccess.Data;
using ITHelpDesk.DataAccess.Repository.IRepository;
using ITHelpDesk.DataAccess.Repository;
using Microsoft.AspNetCore.Identity.UI.Services;
using ITHelpDesk.Utility;
using ITHelpDesk.Models;

using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IO;
using Microsoft.Extensions.FileProviders;
using System.Globalization;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Localization;
using System.Text.Json.Serialization;

namespace HelpDesk
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
            //Localization
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddControllersWithViews().AddViewLocalization();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            
            services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedEmail = true;

            }).AddDefaultTokenProviders()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddSingleton<IEmailSender, EmailSender>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            Global.ConnectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddScoped<IRequest, IRequestService>();
            services.AddScoped<ITHelpDesk.DataAccess.Repository.IPythonBot, PythonBotService>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddControllersWithViews()
                .AddNewtonsoftJson(options =>
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                );
            services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
            services.AddControllersWithViews();
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddRazorPages();
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = $"/Identity/Account/Login";
                options.LogoutPath = $"/Identity/Account/Logout";
                options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
            });

            services.AddControllersWithViews()
                .AddNewtonsoftJson(options =>
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                );

            services.AddAuthentication().AddCookie(cfg => cfg.SlidingExpiration = true)
               .AddJwtBearer(cfg =>
               {
                   cfg.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                   {
                       ValidIssuer = JWT.Issuer,
                       ValidAudience = JWT.Audience,
                       IssuerSigningKey =
                       new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWT.Key))
                   };
               });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //Localiz
            var supportedCultures = new[]
            {
                new CultureInfo("uz"),
                new CultureInfo("ru"),
            };
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("uz"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures,
            });
            // Local
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            /*//shows the process running host
            app.Run(async (context) =>
            {
                await context.Response
                .WriteAsync(System.Diagnostics.Process.GetCurrentProcess().ProcessName);
            });*/

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{area=Requester}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
