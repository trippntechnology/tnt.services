using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using TNT.Services.Service.Data;
using TNT.Services.Service.Models;

namespace TNT.Services.Service
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var host = CreateWebHostBuilder(args).Build();

      using (var scope = host.Services.CreateScope())
			{
        var services = scope.ServiceProvider;
        var logger = services.GetRequiredService<ILogger<Program>>();

        logger.LogInformation("Entering main");

        try
        {
          var context = services.GetRequiredService<ApplicationDbContext>();
          logger.LogInformation("Migrating database");
          context.Database.Migrate();
          logger.LogInformation("Seeding database");
          SeedData.Initialize(services);
				}
				catch (Exception ex)
				{
					logger.LogError(ex, "An error occurred seeding the DB.");
				}
			}

			host.Run();
		}

		public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
				WebHost.CreateDefaultBuilder(args)
						.UseStartup<Startup>();
	}
}
