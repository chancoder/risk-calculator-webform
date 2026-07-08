
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using risk_calculator_webapp.Components;
using System;

namespace risk_calculator_webapp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add Razor Components with Interactive Server Components
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            // Add HttpContextAccessor
            builder.Services.AddHttpContextAccessor();

            // Add Distributed Memory Cache (required before AddSession)
            builder.Services.AddDistributedMemoryCache();

            // Add Session
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            // Add built-in logging
            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();
            builder.Logging.AddDebug();

            var app = builder.Build();

            // 1. Exception handling
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
            }

            // 2. HTTPS redirection
            app.UseHttpsRedirection();

            // 3. Static files
            app.UseStaticFiles();

            // 4. Routing
            app.UseRouting();

            // 7. Session
            app.UseSession();

            // 8. Antiforgery
            app.UseAntiforgery();

            // 9. Endpoint mapping
            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();
        }
    }
}