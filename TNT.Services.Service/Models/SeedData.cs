using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using TNT.Services.Service.Data;

namespace TNT.Services.Service.Models
{
	public static class SeedData
	{
		public static void Initialize(IServiceProvider serviceProvider)
		{
			const string userName = "admin";
			const string passwordHash = @"AQAAAAEAACcQAAAAEL3x+2YzIXlV+ZZ1ne8HZ7BzhhG/bZl9Qb4MqqxtZO5U98gxUjmKL+22jrckTQrGkQ=="; // Passw0rd!
			const string securityStamp = @"XTDF4NCBYHYWQWAWGRBK5TIPGJP76OOR";

			using (var context = new ApplicationDbContext(
							serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
			{
				// Seed admin user
				if (!context.Users.Any())
				{
					context.Users.AddRange(
							new IdentityUser
							{
								UserName = userName,
								NormalizedEmail = userName.ToUpper(),
								NormalizedUserName = userName.ToUpper(),
								PasswordHash = passwordHash,
								SecurityStamp = securityStamp
							}
					);
				}

				context.SaveChanges();
			}
		}
	}
}
