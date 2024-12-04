using Microsoft.AspNetCore.Identity;

namespace StallSync.Data
{
    public class SeedData
    {
        public static async Task SeedRolesAsync(IServiceProvider serviceProvider, UserManager<IdentityUser> userManager)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string[] roleNames = { "Admin", "User", "Manager" };
            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
            var adminUser = await userManager.FindByEmailAsync("admin@stallsync.com");
            if (adminUser == null)
            {
                var user = new IdentityUser
                {
                    UserName = "admin@stallsync.com",
                    Email = "admin@stallsync.com",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(user, "Admin123!");
                await userManager.AddToRoleAsync(user, "Admin");
            }
        }
        
    }
}
