using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TNT.Services.Models.Exceptions;
using TNT.Services.Models.Request;
using TNT.Services.Models.Response;
using TNT.Services.Service.Data;

namespace TNT.Services.Service.Controllers
{
  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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

    [Obsolete("Use GetApplicationInfo")]
    [HttpPost]
    public ApplicationInfo PostApplicationInfo(ApplicationRequest applicationRequest) => GetApplicationInfo(applicationRequest.ApplicationID);

    [HttpGet]
    public ApplicationInfo GetApplicationInfo([FromQuery] int applicationId)
    {
      ApplicationInfo? response = null;

      try
      {
        var application = _context.Application.Where(a => a.ID == applicationId).FirstOrDefault();
        if (application == null) throw new ApplicationNotFoundException(applicationId);

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

    [HttpGet]
    public ReleaseResponse GetRelease([FromQuery] int releaseId)
    {
      return PostRelease(new ReleaseRequest() { ReleaseId = releaseId });
    }

    [HttpPost]
    public ReleaseResponse PostRelease(ReleaseRequest releaseRequest)
    {
      ReleaseResponse response;

      try
      {
        var release = _context.Release.Where(r => r.ID == releaseRequest.ReleaseId).FirstOrDefault() ??
        throw new ReleaseNotFoundException(releaseRequest.ReleaseId, "Release ID, {0}, could not be found");

        response = new ReleaseResponse()
        {
          FileName = release.FileName,
          Package = Convert.ToBase64String(release.Package ?? new byte[0]),
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
    public LicenseeResponse PostVerifyLicense(LicenseeRequest licenseeRequest)
    {
      LicenseeResponse response;

      try
      {
        var licensee = _context.Licensee.Where(l => l.ID == licenseeRequest.LicenseeId && l.ApplicationId == licenseeRequest.ApplicationId).FirstOrDefault() ??
          throw new LicenseeNotFoundException();
        response = new LicenseeResponse()
        {
          ID = licensee.ID,
          Name = licensee.Name,
          ApplicationId = licensee.ApplicationId,
          ValidUntil = licensee.ValidUntil,
        };
      }
      catch (Exception ex)
      {
        response = new LicenseeResponse(ex);
      }

      return response;
    }
  }
}