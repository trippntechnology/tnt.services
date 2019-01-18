using Microsoft.AspNetCore.Mvc;

namespace TNT.Update.Service.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class V1Controller : ControllerBase
	{
		[HttpGet]
		public string TestConnection()
		{
			return "Congratulations! You have successfully connected.";
		}
	}
}