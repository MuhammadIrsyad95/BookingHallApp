using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnePointBooking.Application.Common.Interfaces;
using OnePointBooking.Application.Common.Utility;
using OnePointBooking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePointBooking.Infrastructure.Data
{
    public class DbInitializer : IDbInitializer
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;

        public DbInitializer(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext db)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _db = db;
        }
        public void Initialize()
        {
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
                if (!_roleManager.RoleExistsAsync(SD.Role_Admin).GetAwaiter().GetResult())
                {

                    _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).Wait();
                    _roleManager.CreateAsync(new IdentityRole(SD.Role_Customer)).Wait();

                    ApplicationUser user = _db.ApplicationUsers.FirstOrDefault(u => u.Email == "admin.booking1@gmail.com");
                    if (user == null)
                    {
                        // Create a new user
                        user = new ApplicationUser
                        {
                            UserName = "admin.booking1@gmail.com",
                            Email = "admin.booking1@gmail.com",
                            // Set other required properties here, like password
                        };
                        var result = _userManager.CreateAsync(user, "YourStrongPasswordHere").Result;

                        if (!result.Succeeded)
                        {
                            // Handle errors if user creation fails
                            throw new Exception($"User creation failed: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                        }
                    }

                    // Add user to the Admin role
                    if (!_userManager.IsInRoleAsync(user, SD.Role_Admin).GetAwaiter().GetResult())
                    {
                        _userManager.AddToRoleAsync(user, SD.Role_Admin).GetAwaiter().GetResult();
                    }
                }

            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
