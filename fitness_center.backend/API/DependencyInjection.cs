using API.Authorization.BookingsCancel;
using API.Authorization.ClientsAuthorization;
using API.Authorization.TrainersAuthorization;
using API.Services;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddJWTAuthentication(this IServiceCollection services)
        {

            services.AddScoped<JwtService>();

            services.AddScoped<IAuthorizationHandler, ClientOwnerHandler>()
                    .AddScoped<IAuthorizationHandler, TrainerOwnerHandler>();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("ClientOwnerPolicy", policy =>
                    policy.Requirements.Add(new ClientOwnerRequirement()));
                options.AddPolicy("TrainerOwnerPolicy", policy =>
                    policy.Requirements.Add(new TrainerOwnerRequirement()));
                options.AddPolicy("BookingOwnerPolicy", policy =>
                    policy.Requirements.Add(new BookingOwnerRequirement()));
            });
            return services;
        }
        public static IServiceCollection AddIdentity(this IServiceCollection services)
        {

            services.AddDefaultIdentity<IdentityUser>(options => { })
                .AddRoles<IdentityRole>() // roles
                .AddEntityFrameworkStores<AppDbContext>(); //store

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false; // @ _ и тд 
                options.Password.RequireUppercase = true;

                //защита от перебора паролей
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // уникальность email
                options.User.RequireUniqueEmail = true;

            });

            return services;
        }
    }
}