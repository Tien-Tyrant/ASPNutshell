using System;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreTodo.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCoreTodo
{
    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider services)
        {
            var roleManager = services
            .GetRequiredService<RoleManager<IdentityRole>>();

            await EnsureRoleAsync(roleManager);

            var userManager = services
                .GetRequiredService<UserManager<ApplicationUser>>();
            await EnsureTestAdminAsync(userManager);
            
        }

        private static async Task EnsureTestAdminAsync(UserManager<ApplicationUser> userManager)
        {
            var testAdmin = await userManager.Users
                .Where(x => x.UserName == "admin@todo.local")
                .SingleOrDefaultAsync();

            if (testAdmin != null) return;

            testAdmin = new ApplicationUser
            {
                UserName = "admin@todo.local",
                Email = "admin@todo.local"
            };
            await userManager.CreateAsync(
                    testAdmin, "NotSecure123!!");
            await userManager.AddToRoleAsync(testAdmin, Literals.AdministratorRole);
        }

        private static async Task EnsureRoleAsync(RoleManager<IdentityRole> roleManager)
        {
             var alreadyExists = await roleManager
                .RoleExistsAsync(Literals.AdministratorRole);

            if (alreadyExists) return;

            await roleManager.CreateAsync(
            new IdentityRole(Literals.AdministratorRole));
        }
    }
}