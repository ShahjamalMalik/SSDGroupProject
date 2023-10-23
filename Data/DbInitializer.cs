using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GroupProjectDeployment.Models;
using Azure.Security.KeyVault.Secrets;
using Azure.Core;
using Azure.Identity;

namespace GroupProjectDeployment.Data
{
    public static class DbInitializer
    {
        public static AppSecrets appSecrets { get; set; }
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                context.Database.Migrate();
                // Check if there are already products in the database
                if (context.Products.Any())
                {
                    return; // Database has been seeded
                }

                // Seed the database with 20 products
                var products = new Product[]
                {
                    new Product
                    {
                        Name = "Smart Home Security System",
                        Description = "A comprehensive security system that includes cameras, motion detectors, and remote monitoring for your home.",
                        ImageUrl = "product1.jpg", 
                        Price = 399.99m,
                        quantity = 50
                    },
                    new Product
                    {
                        Name = "Wireless Charging Pad",
                        Description = "A sleek wireless charging pad that's compatible with smartphones, smartwatches, and more.",
                        ImageUrl = "product2.jpeg", 
                        Price = 29.99m,
                        quantity = 100
                    },
                    new Product
                    {
                        Name = "4K Ultra HD TV",
                        Description = "A 4K Ultra HD TV with a stunning display and smart TV features for an immersive viewing experience.",
                        ImageUrl = "product3.jpeg", 
                        Price = 799.99m,
                        quantity = 30
                    },
                    new Product
                    {
                        Name = "High-Performance Laptop",
                        Description = "A high-performance laptop with a powerful processor and a sleek, lightweight design.",
                        ImageUrl = "product4.jpg", 
                        Price = 1199.99m,
                        quantity = 5
                    },
                    new Product
                    {
                        Name = "Wireless Bluetooth Speaker",
                        Description = "Portable Bluetooth speaker with 360-degree sound and a long-lasting battery for music on the go.",
                        ImageUrl = "product5.jpg", 
                        Price = 79.99m,
                        quantity = 90
                    },
                    new Product
                    {
                        Name = "Home Assistant Device",
                        Description = "A voice-controlled home assistant device that can control smart devices, answer questions, and play music.",
                        ImageUrl = "product6.png", 
                        Price = 129.99m,
                        quantity = 40
                    },
                    new Product
                    {
                        Name = "Noise-Canceling Headphones",
                        Description = "Over-ear noise-canceling headphones with exceptional sound quality and comfort for long listening sessions.",
                        ImageUrl = "product7.jpeg", 
                        Price = 199.99m,
                        quantity = 60
                    },
                    new Product
                    {
                        Name = "Ultra-Fast Gaming Router",
                        Description = "A high-performance gaming router with low latency and advanced features for online gaming.",
                        ImageUrl = "product8.jpg", 
                        Price = 149.99m,
                        quantity = 25
                    },
                    new Product
                    {
                        Name = "DSLR Camera Kit",
                        Description = "A DSLR camera kit with lenses and accessories for photography enthusiasts and professionals.",
                        ImageUrl = "product9.jpg", 
                        Price = 999.99m,
                        quantity = 10
                    },
                    new Product
                    {
                        Name = "Smart Doorbell Camera",
                        Description = "A smart doorbell camera that provides real-time video and two-way communication for home security.",
                        ImageUrl = "product10.jpg", 
                        Price = 149.99m,
                        quantity = 35
                    },
                    new Product
                    {
                        Name = "Foldable Electric Scooter",
                        Description = "A foldable electric scooter for convenient and eco-friendly urban transportation.",
                        ImageUrl = "product11.jpeg", 
                        Price = 399.999m,
                        quantity = 85
                    },
                    new Product
                    {
                        Name = "Portable Solid State Drive Drive",
                        Description = "A portable external solid state drive for data storage and backup.",
                        ImageUrl = "product12.jpg", 
                        Price = 79.99m,
                        quantity = 95
                    }
                };

                foreach (var product in products)
                {
                    context.Products.Add(product);
                }

                context.SaveChanges(); // Save changes to the database
            }
        }
        public static async Task<int> SeedUsersAndRoles(IServiceProvider serviceProvider)
        {
            // create the database if it doesn't exist
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            context.Database.Migrate();

            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // Check if roles already exist and exit if there are
            if (roleManager.Roles.Count() > 0)
                return 1;  // should log an error message here

            // Seed roles
            int result = await SeedRoles(roleManager);
            if (result != 0)
                return 2;  // should log an error message here

            // Check if users already exist and exit if there are
            if (userManager.Users.Count() > 0)
                return 3;  // should log an error message here

            // Seed users
            result = await SeedUsers(userManager);
            if (result != 0)
                return 4;  // should log an error message here

            return 0;
        }

        private static async Task<int> SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            // Create Manager Role
            var result = await roleManager.CreateAsync(new IdentityRole("Admin"));
            if (!result.Succeeded)
                return 1;  // should log an error message here

            return 0;
        }

        private static async Task<int> SeedUsers(UserManager<ApplicationUser> userManager)
        {
            // Create Manager User
            var adminUser = new ApplicationUser
            {
                UserName = "admin@webstore.com",
                Email = "admin@webstore.com",
                FirstName = "The",
                LastName = "Admin",
                EmailConfirmed = true
            };
            SecretClientOptions options = new SecretClientOptions()
            {
                Retry =
        {
            Delay= TimeSpan.FromSeconds(2),
            MaxDelay = TimeSpan.FromSeconds(16),
            MaxRetries = 5,
            Mode = RetryMode.Exponential
         }
            };
            var client = new SecretClient(new Uri("https://groupprojectdeploymentva.vault.azure.net/"), new DefaultAzureCredential(), options);

            KeyVaultSecret secret = client.GetSecret("AdminPassword");

            string secretValue = secret.Value;


            var result = await userManager.CreateAsync(adminUser, secretValue);

            // Assign user to Manager role
            result = await userManager.AddToRoleAsync(adminUser, "Admin");
            if (!result.Succeeded)
                return 2;  // should log an error message here

            return 0;
        }
    }
}

