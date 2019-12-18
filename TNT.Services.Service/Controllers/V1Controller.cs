using Microsoft.AspNetCore.Mvc;
using System;
using TNT.Services.Models;
using TNT.Services.Models.Exceptions;
using TNT.Services.Service.Data;
using TNT.Services.Service.Models.Entities;

namespace TNT.Services.Service.Controllers
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
		public Response TestConnection()
		{
			return new Response() { Message = "Congratulations! You have successfully connected." };
		}

		[HttpPost]
		public Response Upload(ReleaseRequest releaseRequest)
		{
			try
			{
				var application = _context.Application.Find(releaseRequest.ApplicationID);

				if (application == null) throw new InvalidApplicationIdException();

				var release = new Release()
				{
					ApplicationID = releaseRequest.ApplicationID,
					Version = releaseRequest.Version.ToString(),
					Package = Convert.FromBase64String(releaseRequest.Base64EncodedFile),
					Date = DateTime.Now,
					FileName = releaseRequest.FileName
				};

				_context.Release.Add(release);
				_context.SaveChanges();

			}
			catch (Exception ex)
			{
				return new Response(ex);
			}

			return new Response();
		}
	}
}