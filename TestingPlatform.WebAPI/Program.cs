using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TestingPlatform.Domain.Entities;
using TestingPlatform.Infrastructure.Context;
using TestingPlatform.WebAPI.Config;

namespace TestingPlatform.WebAPI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration["ConnectionStrings:DefaultConnection"];
            builder.Services.AddDbContext<TestContext>(options =>
                options.UseSqlServer(connectionString).EnableSensitiveDataLogging());
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddAllService(builder.Configuration);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "TestPlatform");

                    c.OAuthClientId("swagger-ui");
                    c.OAuthAppName("Swagger UI");
                });
            }

            app.UseHttpsRedirection();

            app.Use(async (context, next) =>
            {
                var token = context.Request.Cookies["Authorization"];
                if (!string.IsNullOrEmpty(token))
                    context.Request.Headers.Add("Authorization", "Bearer " + token);

                await next();
            });

            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetService<TestContext>();

                dbContext.Database.EnsureCreated();

                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

                string email = builder.Configuration["DefaultUser:email"];
                string userPassword = builder.Configuration["DefaultUser:password"];
                

                if (await userManager.FindByEmailAsync(email) == null)
                {
                    User adminUser = new User
                    {
                        UserName = email,
                        Email = email,
                        EmailConfirmed = true,
                        DateRegistration = DateTime.UtcNow,
                    };

                    IdentityResult result = await userManager.CreateAsync(adminUser, userPassword);
                }

                dbContext.SaveChanges();
            }
            app.UseCors("fromUI");
            
            app.UseAuthentication();
            app.UseAuthorization();
            
            app.MapControllers();

            app.Run();
        }
    }
}