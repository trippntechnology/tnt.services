using Microsoft.AspNetCore.Identity;
using TNT.Services.Service.Data;

namespace TNT.Services.Service.Models
{
  public static class SeedData
  {

    public static void Initialize(ApplicationDbContext dbContext, ILogger logger)
    {
      string email = $"admin@domain.com";
      string normalizedEmail = email.ToUpper();
      string userName = email;
      string normalizedUserName = normalizedEmail;
      const bool emailConfirmed = true;
      const string passwordHash = @"AQAAAAIAAYagAAAAEDRYnALlU9Q/OuGO79YqpPxl4EheFC37uW771wiuSoxVR1glgbXtB8rrRrafn/uUCA=="; // Passw0rd!
      const string securityStamp = @"NDB5U4F5GAW3Q34D23XK7Q7XK2HSKPQG";
      const string concurrentStamp = @"93071d95-aa87-4581-a12b-3273c790fddb";

      if (!dbContext.Users.Any())
      {
        logger.LogInformation("Seeding Users data");
        dbContext.Users.AddRange(
              new IdentityUser()
              {
                UserName = userName,
                NormalizedUserName = normalizedUserName,
                Email = email,
                NormalizedEmail = normalizedEmail,
                EmailConfirmed = emailConfirmed,
                PasswordHash = passwordHash,
                SecurityStamp = securityStamp,
                ConcurrencyStamp = concurrentStamp,
              }
            );
        dbContext.SaveChanges();
      }
    }
  }
}
