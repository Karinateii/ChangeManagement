using Change.DataAccess.Data;
using Change.Models.Models;
using Change.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Change.DataAccess.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;
        private readonly IConfiguration _configuration;
        private readonly ILogger<DbInitializer> _logger;

        public DbInitializer(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext db,
            IConfiguration configuration,
            ILogger<DbInitializer> logger) 
        {      
            _roleManager = roleManager;   
            _userManager = userManager;
            _db = db;
            _configuration = configuration;
            _logger = logger;
        }       

        public void Initialize()
        {
            // Apply pending migrations if they exist
            try
            {
                if (_db.Database.GetPendingMigrations().Any())
                {
                    _logger.LogInformation("Applying pending database migrations...");
                    _db.Database.Migrate();
                    _logger.LogInformation("Database migrations applied successfully.");
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred while applying database migrations.");
                throw; // Re-throw to prevent app startup with database issues
            }

            // Create roles if they don't exist
            try
            {
                if (!_roleManager.RoleExistsAsync(SD.Role_Employee).GetAwaiter().GetResult())
                {
                    _logger.LogInformation("Creating application roles...");
                    _roleManager.CreateAsync(new IdentityRole(SD.Role_Employee)).GetAwaiter().GetResult();
                    _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).GetAwaiter().GetResult();
                    _logger.LogInformation("Application roles created successfully.");

                    // Create admin user from configuration (not hardcoded)
                    var adminEmail = _configuration["AdminUser:Email"];
                    var adminPassword = _configuration["AdminUser:Password"];
                    var adminName = _configuration["AdminUser:Name"];

                    // Only create admin if configuration exists
                    if (!string.IsNullOrEmpty(adminEmail) && !string.IsNullOrEmpty(adminPassword))
                    {
                        var adminUser = new ApplicationUser
                        {
                            UserName = adminEmail,
                            Email = adminEmail,
                            Name = adminName ?? "System Administrator",
                            EmailConfirmed = true,
                            PhoneNumber = _configuration["AdminUser:PhoneNumber"],
                            StreetAddress = _configuration["AdminUser:StreetAddress"],
                            State = _configuration["AdminUser:State"],
                            PostalCode = _configuration["AdminUser:PostalCode"],
                            City = _configuration["AdminUser:City"]
                        };

                        var result = _userManager.CreateAsync(adminUser, adminPassword).GetAwaiter().GetResult();
                        
                        if (result.Succeeded)
                        {
                            var user = _db.ApplicationUsers.FirstOrDefault(u => u.Email == adminEmail);
                            if (user != null)
                            {
                                _userManager.AddToRoleAsync(user, SD.Role_Admin).GetAwaiter().GetResult();
                                _logger.LogInformation("Admin user created successfully.");
                            }
                        }
                        else
                        {
                            _logger.LogWarning("Failed to create admin user: {Errors}", string.Join(", ", result.Errors.Select(e => e.Description)));
                        }
                    }
                    else
                    {
                        _logger.LogWarning("Admin user configuration not found. Please configure AdminUser section in appsettings or user secrets.");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while initializing roles and admin user.");
                throw;
            }
        }
    }
}
