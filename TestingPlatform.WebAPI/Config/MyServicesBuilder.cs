using Microsoft.AspNetCore.Identity;
using TestingPlatform.Application.Interfaces;
using TestingPlatform.Application.Services;
using TestingPlatform.Domain.Entities;
using TestingPlatform.Domain.Interfaces;
using TestingPlatform.Infrastructure.Context;
using TestingPlatform.Infrastructure.Interfaces;
using TestingPlatform.Infrastructure.Repository;
using TestingPlatform.Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Configuration;
using TestingPlatform.Application.Options;

namespace TestingPlatform.WebAPI.Config
{
    public static class MyServicesBuilder
    {
        public static IServiceCollection AddAllService(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddingOwnDI(configuration);
            services.AddConfIdentity();
            services.AddAutoMapper(typeof(TestingPlatform.Application.Mapping.MappingProfile));
            services.AddSwager();
            services.AddCors();
            services.AddJwt(configuration);

            return services;
        }

        public static IServiceCollection AddingOwnDI(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddMemoryCache();
            services.AddSingleton<IRepositoryFactory, RepositoryFactory>();
            services.AddScoped<IUnitOfWork, UnitOfWork<TestContext>>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITestService, TestService>();
            services.Configure<TokenOption>(configuration.GetSection("JwtSettings"));
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUserService, UserService>();

            return services;
        }

        public static IServiceCollection AddConfIdentity(this IServiceCollection services)
        {
            services.AddIdentity<User, Role>(option =>
            {
                option.SignIn.RequireConfirmedEmail = true;
            }).AddEntityFrameworkStores<TestContext>()
               .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = true;
            });
            
            return services;
        }

        public static IServiceCollection AddJwt(
            this IServiceCollection services,
            IConfiguration configuration)
        {

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = Boolean.Parse(configuration["JwtSettings:ValidateIssuer"]),
                    ValidateAudience = Boolean.Parse(configuration["JwtSettings:ValidateAudience"]),
                    ValidateIssuerSigningKey = Boolean.Parse(configuration["JwtSettings:ValidateIssuerSigningKey"]),
                    ValidIssuer = configuration["JwtSettings:Issuer"],
                    ValidAudience = configuration["JwtSettings:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:SecurityKey"]))
                };
            });

            return services;
        }
        public static IServiceCollection AddCors(this IServiceCollection service)
        {
            var nameMyPolicy = "fromUI";
            service.AddCors(options =>
            {
                options.AddPolicy(nameMyPolicy, policy =>
                {
                    policy.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
            return service;
        }

        public static IServiceCollection AddSwager(
            this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TestPlatform", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                   Description = "JWT Authorization header using the Bearer scheme",
                   Name = "Authorization",
                   In = ParameterLocation.Header,
                   Type = SecuritySchemeType.ApiKey
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                        },
                        new string[] {}
                    }
                });
            });

            return services;
        }
    }
}
