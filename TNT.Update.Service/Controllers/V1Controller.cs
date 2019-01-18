using Microsoft.AspNetCore.Mvc;
using System;
using TNT.Update.Models;
using TNT.Update.Service.Data;
using TNT.Update.Service.Models;
using TNT.Update.Service.Models.Entities;

namespace TNT.Update.Service.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class V1Controller : ControllerBase
	{
		private readonly ApplicationDbContext _context;

		public V1Controller(ApplicationDbContext context)
		{
			_context = context;
		}

		[HttpGet]
		public string TestConnection()
		{
			return "Congratulations! You have successfully connected.";
		}

		[HttpPost]
		public string Upload(ReleaseRequest releaseRequest)
		{
			var release = new Release()
			{
				ApplicationID = releaseRequest.ApplicationID,
				Version = releaseRequest.Version.ToString(),
				Package = Convert.FromBase64String(releaseRequest.Base64EncodedFile)
			};

			_context.Release.Add(release);
			_context.SaveChanges();

			return "success";
		}
	}
}