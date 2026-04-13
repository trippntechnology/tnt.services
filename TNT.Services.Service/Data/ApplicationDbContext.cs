using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TNT.Services.Service.Models.Entities;

namespace TNT.Services.Service.Data;

/// <summary>
/// Entity Framework Core database context for the TNT Services Razor Pages application.
/// Inherits from <see cref="IdentityDbContext"/> to provide ASP.NET Core Identity tables along with custom domain entities.
/// </summary>
/// <remarks>
/// <para>
/// This context manages four primary entity sets:
/// <list type="bullet">
/// <item><description><see cref="Application"/> - Registered applications in the system</description></item>
/// <item><description><see cref="Release"/> - Release packages and versions</description></item>
/// <item><description><see cref="Licensee"/> - Licensed consumers</description></item>
/// <item><description><see cref="Analytic"/> - Analytics data associated with applications</description></item>
/// </list>
/// </para>
/// <para>
/// To create and apply database migrations, use one of the following approaches:
/// </para>
/// <para>
/// .NET CLI (recommended):
/// <code>
/// dotnet ef migrations add MigrationName
/// dotnet ef migrations remove
/// dotnet ef database update [-v]
/// </code>
/// </para>
/// <para>
/// Package Manager Console:
/// <code>
/// Add-Migration MigrationName
/// Update-Database
/// </code>
/// </para>
/// </remarks>
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext(options)
{
    /// <summary>
    /// Gets or sets the collection of applications registered in the system.
    /// </summary>
    public virtual DbSet<Application> Application { get; set; }

    /// <summary>
    /// Gets or sets the collection of release packages associated with applications.
    /// </summary>
    public virtual DbSet<Release> Release { get; set; }

    /// <summary>
    /// Gets or sets the collection of licensees tied to applications.
    /// </summary>
    public virtual DbSet<Licensee> Licensee { get; set; }

    /// <summary>
    /// Gets or sets the collection of analytics data associated with applications.
    /// </summary>
    public virtual DbSet<Analytic> Analytics { get; set; }
}
