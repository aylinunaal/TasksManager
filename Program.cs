using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using taskManager.Services;
using taskManager.Data;
using Newtonsoft.Json.Serialization;

namespace taskManager
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureServices((context, services) =>
                    {
                        services.AddControllers()
                            .AddNewtonsoftJson(options =>
                            {
                                options.SerializerSettings.ContractResolver = new DefaultContractResolver
                                {
                                    NamingStrategy = null // Set the desired naming strategy or leave null for default
                                };
                            });

                        // Register your services
                        services.AddScoped<AuthService>();
                        services.AddScoped<JwtService>(provider => new JwtService("bD9hG3cXv5zL8uW1oT6rP7qN2jM4yK0VbD9hG3cXv5zL8uW1oT6rP7qN2jM4yK0Vaaa"));
                        services.AddScoped<DataContext>(); // Ensure DataContext is registered
                    });
                    webBuilder.UseStartup<Startup>();
                });
    }
}
