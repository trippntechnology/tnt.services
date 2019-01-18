using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TNT.Update.Service.Data
{
	public class ApplicationDbContext : IdentityDbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
				: base(options)
		{
		}
		public DbSet<TNT.Update.Service.Models.Application> Application { get; set; }

		public DbSet<TNT.Update.Service.Models.Release> Release { get; set; }
	}
}
