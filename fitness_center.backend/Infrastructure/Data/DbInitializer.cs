using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public static class DbInitializer
    {
        /// <summary>
        /// поменять в аппсеттинг пароль и имеил если нужно
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static async Task InitializeAsync(this IServiceProvider serviceProvider, IConfiguration configuration)
        {
            var scope = serviceProvider.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string[] role = { "Admin", "Client", "Trainer" };
            foreach (var roleItem in role)
            {
                if (!await roleManager.RoleExistsAsync(roleItem))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleItem));
                }
            }

            var adminEmail = configuration["AdminSettings:Email"] ?? "admin@fitness.com";
            

            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var adminUser = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true // подтверждаем email
                };

                var adminPassword = configuration["AdminSettings:Password"] ?? "Admin123!";
                var result = await userManager.CreateAsync(adminUser, adminPassword);
                if (result.Succeeded) {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string[] roles = { "Admin", "Client", "Trainer" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            var adminEmail = "admin@fitness.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(adminUser, "Admin123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                    Console.WriteLine("Admin created and role assigned!");
                }
                else
                {
                    foreach (var error in result.Errors)
                        Console.WriteLine($"Error: {error.Description}");
                }
            }
            else
            {
                if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
                    await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }
}
