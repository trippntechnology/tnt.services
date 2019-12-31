using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using TNT.Services.Models;
using TNT.Services.Models.Exceptions;
using TNT.Services.Models.Request;
using TNT.Services.Models.Response;
using TNT.Services.Service.Data;
using TNT.Services.Service.Models.Entities;

namespace TNT.Services.Service.Controllers
{
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
				var release = _context.Release.Find(releaseRequest.ReleaseId);
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

		[HttpPost]
		public ReleaseResponse GetUpdate(UpdateRequest updateRequest)
		{
			try
			{
				if (updateRequest == null) throw new NullReferenceException("UpdateRequest can not be null");

				var application = _context.Application.Find(updateRequest.ApplicationID);

				if (application == null) throw new InvalidApplicationIdException();

				var release = _context.Release.Where(r => r.ApplicationID == application.ID && r.Version.ToVersion() > updateRequest.CurrentVersion.ToVersion()).ToList();

			}
			catch (Exception ex)
			{
				return new ReleaseResponse(ex);
			}

			return new ReleaseResponse();
		}

		//	[HttpPost]
		//	public Response Upload(ReleaseRequest releaseRequest)
		//	{
		//		try
		//		{
		//			var application = _context.Application.Find(releaseRequest.ApplicationID);

		//			if (application == null) throw new InvalidApplicationIdException();

		//			var package = Convert.FromBase64String(releaseRequest.Base64EncodedFile);

		//			var release = new Release()
		//			{
		//				ApplicationID = releaseRequest.ApplicationID,
		//				Version = GetVersion(package),
		//				Package = package,
		//				Date = DateTime.Now,
		//				FileName = releaseRequest.FileName
		//			};

		//			_context.Release.Add(release);
		//			_context.SaveChanges();

		//		}
		//		catch (Exception ex)
		//		{
		//			return new Response(ex);
		//		}

		//		return new Response();
		//	}
	}
}