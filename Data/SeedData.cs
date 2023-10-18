using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SakeFigureShop.Domains;

namespace SakeFigureShop.Data
{
    public class SeedData
    {
        public static async Task Initialize(
            IServiceProvider serviceProvider, string username = "admin@example.com", string pw = "Admin@1234")
        {
            using (
                var context = new ApplicationDbContext(serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>())
            ) {
                var adminUid = await EnsureUser(serviceProvider, username, pw);

                await EnsureRole(serviceProvider, adminUid, "Admin");
            }
        }
        private static async Task<string> EnsureUser(
            IServiceProvider serviceProvider,
            string username,
            string initPw) 
        {
            var userManager = serviceProvider.GetService<UserManager<User>>();
            var user = await userManager.FindByNameAsync(username);
            if (user == null)
            {
                user = new User { 
                    UserName = username,
                    Email = username,
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(user, initPw);
            }
            if (user == null)
            {
                throw new Exception("User did not created. Password Policy?");
            }


            return user.Id;
        }

        private static async Task<IdentityResult> EnsureRole(
            IServiceProvider serviceProvider,
            string uid,
            string role)
        {
            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

            IdentityResult ir;

            if (await roleManager.RoleExistsAsync(role) == false)
            {
                ir = await roleManager.CreateAsync(new IdentityRole(role));
            }

            var userManager = serviceProvider.GetService<UserManager<User>>();

            var user = await userManager.FindByIdAsync(uid);
            if (user == null)
            {
                throw new Exception("User not existing.");
            }

            ir = await userManager.AddToRoleAsync(user, role);

            return ir;
        }
    }
}
