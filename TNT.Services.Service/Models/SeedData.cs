using Microsoft.AspNetCore.Identity;
using TNT.Services.Service.Data;

namespace TNT.Services.Service.Models;

/// <summary>
/// Provides methods for seeding initial data into the database, such as default users.
/// </summary>
public static class SeedData
{
    /// <summary>
    /// Seeds an admin user into the database if one does not already exist.
    /// </summary>
    /// <param name="serviceProvider">The service provider used to resolve dependencies.</param>
    /// <param name="logger">The logger used for logging information.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public static async Task InitializeAsync(IServiceProvider serviceProvider, ILogger logger)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
        string email = "admin@domain.com";
        if (await userManager.FindByEmailAsync(email) == null)
        {
            logger.LogInformation("Seeding Users data");
            var user = new IdentityUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true
            };
            await userManager.CreateAsync(user, "Passw0rd!");
        }
    }
}
