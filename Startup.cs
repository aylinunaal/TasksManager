
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Text;
using taskManager.Data;
using taskManager.Services;

namespace taskManager
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // Veritabanı yapılandırması (In-Memory Database)
            services.AddDbContext<DataContext>(options =>
                options.UseInMemoryDatabase("JwtAuthExampleDb"));

            // JWT kimlik doğrulama yapılandırması
            var key = Encoding.ASCII.GetBytes("bD9hG3cXv5zL8uW1oT6rP7qN2jM4yK0VbD9hG3cXv5zL8uW1oT6rP7qN2jM4yK0Vaaa");  // Anahtar burada sabit bir string olarak verilmiştir.
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.FromMinutes(5)
                };
            });

            // Servislerin DI (Dependency Injection) ile eklenmesi
            services.AddScoped<TaskService>();
            services.AddScoped<AuthService>();

            services.AddControllers();

            // Swagger yapılandırması
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Task Manager API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Write your JWT Authorization token as in like in example . Example: \"Bearer {Your_token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            // Kimlik doğrulama ve yetkilendirme middleware'lerinin eklenmesi
            app.UseAuthentication();
            app.UseAuthorization();

            // Swagger middleware'inin eklenmesi
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
