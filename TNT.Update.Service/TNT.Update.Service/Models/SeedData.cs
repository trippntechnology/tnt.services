using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TNT.Update.Service.Data;

namespace TNT.Update.Service.Models
{
	public static class SeedData
	{
		public static void Initialize(IServiceProvider serviceProvider)
		{
			const string userName = "admin@domain.com";
			const string passwordHash = @"AQAAAAEAACcQAAAAEL3x+2YzIXlV+ZZ1ne8HZ7BzhhG/bZl9Qb4MqqxtZO5U98gxUjmKL+22jrckTQrGkQ==";
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
								Email = userName,
								NormalizedUserName = userName.ToUpper(),
								PasswordHash = passwordHash,
								SecurityStamp = securityStamp
							}
					);
				}

				//	// Seed carriers
				//	if (!context.Carrier.Any())
				//	{
				//		context.Carrier.AddRange(
				//				new Carrier { Domain = "message.alltel.com", Name = "Alltel" },
				//				new Carrier { Domain = "txt.att.net", Name = "AT&T" },
				//				new Carrier { Domain = "myboostmobile.com", Name = "Boost Mobile" },
				//				new Carrier { Domain = "mms.cricketwireless.net", Name = "Cricket Wireless" },
				//				new Carrier { Domain = "msg.fi.google.com", Name = "Project Fi" },
				//				new Carrier { Domain = "messaging.sprintpcs.com", Name = "Sprint" },
				//				new Carrier { Domain = "email.uscc.net", Name = "U.S. Cellular" },
				//				new Carrier { Domain = "vmobl.com", Name = "Virgin Mobile" },
				//				new Carrier { Domain = "tmomail.net", Name = "T-Mobile" },
				//				new Carrier { Domain = "vtext.com", Name = "Verizon" },
				//				new Carrier { Domain = "text.republicwireless.com", Name = "Republic Wireless" }
				//		);
				//	}

				//	// Seed settings
				//	if (!context.Setting.Any())
				//	{
				//		context.Setting.AddRange(
				//				new Setting { Name = Constants.SETTINGS.AUTHENTICATION_TOKEN, Value = Guid.NewGuid().ToString() },
				//				new Setting { Name = Constants.SETTINGS.REQUEST_TIME_TO_LIVE, Value = "120" },
				//				new Setting { Name = Constants.SETTINGS.SMTP_HOST },
				//				new Setting { Name = Constants.SETTINGS.SMTP_PORT },
				//				new Setting { Name = Constants.SETTINGS.SMTP_USERNAME },
				//				new Setting { Name = Constants.SETTINGS.SMTP_PASSWORD },
				//				new Setting { Name = Constants.SETTINGS.SMTP_ENABLE_SSL, Value = "false" },
				//				new Setting { Name = Constants.SETTINGS.TOKEN_TIME_TO_LIVE, Value = "120" },
				//				new Setting { Name = Constants.SETTINGS.DEVELOPER_MODE, Value = "true" }
				//		);
				//	}

				//	// Seed a relay
				//	if (!context.Relay.Any())
				//	{
				//		context.Relay.Add(
				//			new TNT.Relay.Models.Relay { Description = "Relay State 1", Parameter = "relay1State", PulseTime = 0, URI = "http://192.168.0.5/state.xml" }
				//		);
				//	}

				context.SaveChanges();
			}
		}
	}
}
