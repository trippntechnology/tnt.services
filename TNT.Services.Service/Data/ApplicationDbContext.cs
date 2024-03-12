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
    public virtual DbSet<Application> Application { get; set; }

    public virtual DbSet<Release> Release { get; set; }

    public virtual DbSet<Licensee> Licensee { get; set; }
  }
}
