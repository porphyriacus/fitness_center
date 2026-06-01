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
        
    }
}