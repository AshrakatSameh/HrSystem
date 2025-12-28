using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TamweelyHr.Domain.Entities;

namespace TamweelyHR.Infrastructure.Data
{
    public class DbInitializer
    {
        public static async Task InitializeAsync(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager, // Specify the generic type argument for UserManager
        RoleManager<IdentityRole> roleManager) // Specify the generic type argument for RoleManager
        {
            await context.Database.MigrateAsync();

            // Seed roles
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }
            if (!await roleManager.RoleExistsAsync("User"))
            {
                await roleManager.CreateAsync(new IdentityRole("User"));
            }

            // Seed admin user
            if (await userManager.FindByNameAsync("admin") == null)
            {
                var admin = new ApplicationUser
                {
                    UserName = "admin",
                    Email = "admin@tamweely.com",
                    FullName = "System Administrator",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(admin, "Admin@123");
                await userManager.AddToRoleAsync(admin, "Admin");
            }

            // Seed regular user
            if (await userManager.FindByNameAsync("user") == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "user",
                    Email = "user@tamweely.com",
                    FullName = "Regular User",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(user, "User@123");
                await userManager.AddToRoleAsync(user, "User");
            }

            // Seed departments
            if (!context.Departments.Any())
            {
                context.Departments.AddRange(
                    new Department { Name = "IT", Description = "Information Technology" },
                    new Department { Name = "HR", Description = "Human Resources" },
                    new Department { Name = "Finance", Description = "Finance Department" }
                );
                await context.SaveChangesAsync();
            }

            // Seed jobs
            if (!context.JobTitles.Any())
            {
                context.JobTitles.AddRange(
                    new Job { Title = "Developer", Description = "Software Developer" },
                    new Job { Title = "Manager", Description = "Department Manager" },
                    new Job { Title = "Accountant", Description = "Financial Accountant" }
                );
                await context.SaveChangesAsync();
            }
        }
    }
}
