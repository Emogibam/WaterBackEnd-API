using AquaWater.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using RomanyWaterAPI.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomanyWaterAPI.Data.Seed
{
    public class Seeder
    {
        public async static Task Seed(RoleManager<IdentityRole> roleManager, UserManager<User> userManager, AppDbContext dbContext)
        {
            await dbContext.Database.EnsureCreatedAsync();
            await SeedRolesAsync(roleManager, dbContext);
            await SeedUserAsync(userManager, dbContext);
            await SeedAdminUserAsync(userManager, dbContext);
            await SeedCustomerAsync(userManager, dbContext);
            await SeedCompanyManagerAsync(userManager, dbContext);
            await SeedProductAsync(dbContext);
            await SeedProductGalleryAsync(dbContext);
            await SeedRatingAsync(dbContext);
            await SeedReviewAsync(dbContext);
        }

        private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager, AppDbContext dbContext)
        {
            if (!dbContext.Roles.Any())
            {
                var roles = SeederHelper<string>.GetData("Roles.json");
                foreach (var role in roles)
                {
                    var success = await roleManager.CreateAsync(new IdentityRole { Name = role });
                }
            }
        }
        private static async Task SeedUserAsync(UserManager<User> userManager, AppDbContext dbContext)
        {
            if (!dbContext.Users.Any())
            {
                var users = SeederHelper<User>.GetData("User.json");
                foreach (var user in users)
                {
                    user.EmailConfirmed = true;
                    await userManager.CreateAsync(user, "Auqa@123");
                }
            }
        }

        private static async Task SeedAdminUserAsync(UserManager<User> userManager, AppDbContext dbContext)
        {
            if (!dbContext.AdminUsers.Any())
            {
                var admins = SeederHelper<AdminUser>.GetData("Admin.json");
                await dbContext.AdminUsers.AddRangeAsync(admins);
                await dbContext.SaveChangesAsync();

                foreach (var admin in admins)
                {
                    var user = await userManager.FindByIdAsync(admin.UserId.ToString());
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }
        }

        private static async Task SeedCustomerAsync(UserManager<User> userManager, AppDbContext dbContext)
        {
            if (!dbContext.Customers.Any())
            {
                var customers = SeederHelper<Customer>.GetData("Customer.json");
                await dbContext.Customers.AddRangeAsync(customers);
                await dbContext.SaveChangesAsync();

                foreach (var customer in customers)
                {
                    var customerExist = await userManager.FindByIdAsync(customer.UserId.ToString());
                    await userManager.AddToRoleAsync(customerExist, "Customer");
                }
            }
        }
        private static async Task SeedCompanyManagerAsync(UserManager<User> userManager, AppDbContext dbContext)
        {
            if (!dbContext.CompanyManagers.Any())
            {
                var managers = SeederHelper<CompanyManager>.GetData("CompanyManager.json");


                foreach (var item in managers)
                {
                    dbContext.CompanyManagers.Add(item);
                    await dbContext.SaveChangesAsync();
                }
                foreach (var manager in managers)
                {
                    var managerExist = await userManager.FindByIdAsync(manager.UserId.ToString());
                    await userManager.AddToRoleAsync(managerExist, "CompanyManager");
                }
            }
        }
        private static async Task SeedProductAsync(AppDbContext dbContext)
        {
            if (!dbContext.Products.Any())
            {
                var products = SeederHelper<Product>.GetData("Products.json");
                await dbContext.Products.AddRangeAsync(products);
                await dbContext.SaveChangesAsync();
            }
        }
        private static async Task SeedProductGalleryAsync(AppDbContext dbContext)
        {
            if (!dbContext.ProductGalleries.Any())
            {
                var productGalleries = SeederHelper<ProductGallery>.GetData("ProductGallery.json");
                await dbContext.ProductGalleries.AddRangeAsync(productGalleries);
                await dbContext.SaveChangesAsync();
            }
        }
        private static async Task SeedRatingAsync(AppDbContext dbContext)
        {
            if (!dbContext.Ratings.Any())
            {
                var ratings = SeederHelper<Rating>.GetData("Rating.json");
                await dbContext.Ratings.AddRangeAsync(ratings);
                await dbContext.SaveChangesAsync();
            }
        }
        private static async Task SeedReviewAsync(AppDbContext dbContext)
        {
            if (!dbContext.Reviews.Any())
            {
                var reviews = SeederHelper<Review>.GetData("Review.json");
                await dbContext.Reviews.AddRangeAsync(reviews);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
