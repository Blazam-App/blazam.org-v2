using System.Globalization;
using ApplicationNews;
using blazam.org.Data;
using blazam.org.Data.Plugins;
using BLAZAM.Notifications.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;

namespace blazam.org
{
    public class Program
    {
        public static IConfiguration? Configuration { get; private set; }

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            Configuration = builder.Configuration;

            NewsDbContext.ConnectionString = Configuration.GetConnectionString("DbConnectionString");
            // Add services to the container.
            builder.Services.AddRazorPages();

            builder.Services.AddHttpContextAccessor();

            builder.Services.AddServerSideBlazor();
            builder.Services.AddSingleton<WeatherForecastService>();
            builder.Services.AddMudServices(options =>
            {
                options.SnackbarConfiguration.HideTransitionDuration = 25;
                options.SnackbarConfiguration.ShowTransitionDuration = 25;

            });
            builder.Services.AddScoped<AppSnackBarService>();
            //Set up string localization
            builder.Services.AddLocalization();
            builder.Services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("en-US"),
                    new CultureInfo("fr-FR")
                 };

                options.DefaultRequestCulture = new RequestCulture("fr-FR");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

            builder.Services.AddScoped<AppAuthenticationStateProvider>();
            //Set up authentication and api token authentication
            builder.Services.Configure<CookiePolicyOptions>(options =>
            {

                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            builder.Services.AddAuthentication(
                CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(AppAuthenticationStateProvider.ApplyAuthenticationCookieOptions());

            //var factory = new NewsDatabaseContextFactory(options.Options);
            //builder.Services.AddSingleton<NewsDatabaseContextFactory>(factory);
            builder.Services.AddDbContextFactory<NewsDbContext>();


            // Add plugins database
            var pluginsConnectionString = builder.Configuration.GetConnectionString("PluginsConnection");
            builder.Services.AddDbContextFactory<PluginsDbContext>(options =>
                options.UseSqlServer(pluginsConnectionString));
            PluginsDbContext.ConnectionString = pluginsConnectionString;

            // Register plugin services
            builder.Services.AddScoped<EmailService>();
            builder.Services.AddScoped<PluginAuthService>();





            var app = builder.Build();

            // Apply migrations on startup
            using (var scope = app.Services.CreateScope())
            {
                var pluginsContext = scope.ServiceProvider.GetRequiredService<PluginsDbContext>();
                pluginsContext.Database.Migrate();
            }

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
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
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });

            app.Run();
        }
    }
}