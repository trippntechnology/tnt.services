using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using TNT.Services.Models.Exceptions;
using TNT.Services.Models.Request;
using TNT.Services.Models.Response;
using TNT.Services.Service.Data;

namespace TNT.Services.Service.Controllers
{
	[Authorize]
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class V1Controller : BaseController
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
		public ApplicationInfo GetApplicationInfo(ApplicationRequest applicationRequest)
		{
			ApplicationInfo response = null;

			try
			{
				var application = _context.Application.Where(a => a.ID == applicationRequest.ApplicationID).FirstOrDefault();
				if (application == null) throw new ApplicationNotFoundException(applicationRequest.ApplicationID);

				var release = _context.Release.Where(r => r.ApplicationID == application.ID).OrderByDescending(r => r.Date).FirstOrDefault();
				if (release == null) throw new ReleaseNotFoundException(application.ID);

				response = new ApplicationInfo
				{
					Name = application.Name,
					ReleaseID = release.ID,
					ReleaseDate = release.Date,
					ReleaseVersion = release.Version
				};
			}
			catch (Exception ex)
			{
				response = new ApplicationInfo(ex);
			}

			return response;
		}

		[HttpPost]
		public ReleaseResponse GetRelease(ReleaseRequest releaseRequest)
		{
			ReleaseResponse response;

			try
			{
				var release = _context.Release.Where(r => r.ID == releaseRequest.ReleaseId).FirstOrDefault();
				if (release == null) throw new ReleaseNotFoundException(releaseRequest.ReleaseId, "Release ID, {0}, could not be found");

				response = new ReleaseResponse()
				{
					FileName = release.FileName,
					Package = Convert.ToBase64String(release.Package),
					ReleaseDate = release.Date
				};
			}
			catch (Exception ex)
			{
				response = new ReleaseResponse(ex);
			}

			return response;
		}
	}
}