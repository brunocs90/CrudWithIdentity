using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using OverviewIdentity.Data;
using OverviewIdentity.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace OverviewIdentity
{
    public class Startup : IStartup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddDefaultIdentity<IdentityUser>()
                .AddDefaultUI()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddAuthentication()
                   .AddFacebook(options =>
                   {
                       options.AppId = Configuration["Authentication:Facebook:AppId"];
                       options.AppSecret = Configuration["Authentication:Facebook:AppSecret"];
                   })
                   .AddGoogle(options =>
                   {
                       options.ClientId = Configuration["Authentication:Google:ClientId"];
                       options.ClientSecret = Configuration["Authentication:Google:ClientSecret"];
                       options.SaveTokens = true;
                   })
                   .AddLinkedIn(options =>
                   {
                       options.ClientId = Configuration["Authentication:LinkedIn:ClientId"];
                       options.ClientSecret = Configuration["Authentication:LinkedIn:ClientSecret"];
                       options.SaveTokens = true;
                   });

            services.AddDatabaseDeveloperPageExceptionFilter(); 
            services.AddControllersWithViews();
        }

        public void Configure(WebApplication app, IWebHostEnvironment environment)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
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

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");            

            app.MapRazorPages();
        }
    }

    public interface IStartup
    {
        IConfiguration Configuration { get; }

        void Configure(WebApplication app, IWebHostEnvironment environment);

        void ConfigureServices(IServiceCollection services);
    }

    public static class StartupExtensions
    {
        public static WebApplicationBuilder UseStartup<TStartuo>(this WebApplicationBuilder webApplicationBuilder) where TStartuo : IStartup
        {
            var startup = Activator.CreateInstance(typeof(TStartuo), webApplicationBuilder.Configuration) as IStartup;
            if (startup == null)
                throw new ArgumentException("Classe startup.cs inválida");

            startup.ConfigureServices(webApplicationBuilder.Services);

            var app = webApplicationBuilder.Build();
            startup.Configure(app, app.Environment);
            app.Run();

            return webApplicationBuilder;
        }
    }
}
