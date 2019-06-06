using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(TNT.Services.Service.Areas.Identity.IdentityHostingStartup))]
namespace TNT.Services.Service.Areas.Identity
{
	public class IdentityHostingStartup : IHostingStartup
	{
		public void Configure(IWebHostBuilder builder)
		{
			builder.ConfigureServices((context, services) =>
			{
			});
		}
	}
}