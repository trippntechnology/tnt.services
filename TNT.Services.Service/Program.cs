using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TNT.Services.Service.Data;
using TNT.Services.Service.Models;

namespace TNT.Services.Service
{
  public class Program
  {
    public static void Main(string[] args)
    {
      var builder = WebApplication.CreateBuilder(args);

      // Add services to the container.
      var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
      builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(connectionString));
      builder.Services.AddDatabaseDeveloperPageExceptionFilter();
      builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
          .AddEntityFrameworkStores<ApplicationDbContext>();
      builder.Services.AddControllersWithViews();

      builder.Services.AddAuthentication()
         .AddJwtBearer(options =>
         {
           //options.IncludeErrorDetails = true;
           options.RequireHttpsMetadata = false;
           options.SaveToken = true;
           options.TokenValidationParameters = new TokenValidationParameters()
           {
             ValidateIssuer = true,
             ValidateAudience = true,
             ValidateLifetime = true,
             ValidAudience = builder.Configuration["Jwt:Audience"],
             ValidIssuer = builder.Configuration["Jwt:Issuer"],
             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? string.Empty))
           };
         });

      var app = builder.Build();
      app.Logger.LogInformation("Logging from Program::Main");

      using (var scope = app.Services.CreateScope())
      {
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        dbContext.Database.EnsureCreated();
        //app.Logger.LogInformation("Migrating database");
        //dbContext.Database.Migrate();
        app.Logger.LogInformation("Seeding database");
        SeedData.Initialize(dbContext, app.Logger);
      }

      // Configure the HTTP request pipeline.
      if (app.Environment.IsDevelopment())
      {
        app.UseMigrationsEndPoint();
      }
      else
      {
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
      }

      app.UseHttpsRedirection();
      app.UseStaticFiles();

      app.UseRouting();

      app.UseAuthentication();
      app.UseAuthorization();
      app.MapControllers();

      app.MapControllerRoute(
          name: "default",
          pattern: "{controller=Home}/{action=Index}/{id?}");
      app.MapRazorPages();

      app.Run();
    }
  }
}
