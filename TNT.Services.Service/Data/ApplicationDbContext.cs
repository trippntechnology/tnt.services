using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TNT.Services.Service.Models.Entities;

namespace TNT.Services.Service.Data
{
	public class ApplicationDbContext : IdentityDbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
				: base(options)
		{
		}
		public DbSet<Application> Application { get; set; }

		public DbSet<Release> Release { get; set; }
	}
}
