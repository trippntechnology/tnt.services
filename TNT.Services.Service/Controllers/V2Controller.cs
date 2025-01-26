using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TNT.Services.Models;
using TNT.Services.Models.Dto;
using TNT.Services.Models.Exceptions;
using TNT.Services.Service.Data;

namespace TNT.Services.Service.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("api/[controller]/[action]")]
[ApiController]
public class V2Controller : BaseController
{
  private readonly ApplicationDbContext _context;

  public V2Controller(ApplicationDbContext context)
  {
    _context = context;
  }

  [HttpGet]
  public DtoResponse<string> Test()
  {
    return new DtoResponse<string>("Congratulations! You have successfully connected.");
  }

  [HttpGet]
  public DtoResponse<ApplicationInfoDto> ApplicationInfo([FromQuery] Guid applicationId)
  {
    try
    {
      var application = _context.Application.Where(a => a.ID == applicationId).FirstOrDefault();
      if (application == null) throw new ApplicationNotFoundException(applicationId);

      var release = _context.Release.Where(r => r.ApplicationID == application.ID).OrderByDescending(r => r.Date).FirstOrDefault();
      if (release == null) throw new ReleaseNotFoundException(application.ID);

      ApplicationInfoDto dto = new ApplicationInfoDto
      {
        ApplicationId = application.ID,
        Name = application.Name,
        ReleaseID = release.ID,
        ReleaseDate = release.Date,
        ReleaseVersion = release.Version
      };

      return new DtoResponse<ApplicationInfoDto>(dto);
    }
    catch (Exception ex)
    {
      return new DtoResponse<ApplicationInfoDto>(ex);
    }
  }

  [HttpGet]
  public DtoResponse<ReleaseInfoDto> ReleaseInfo([FromQuery] int releaseId)
  {
    try
    {
      var release = _context.Release.Where(r => r.ID == releaseId).FirstOrDefault() ??
      throw new ReleaseNotFoundException(releaseId, "Release ID, {0}, could not be found");

      ReleaseInfoDto dto = new ReleaseInfoDto()
      {
        Id = release.ID,
        FileName = release.FileName,
        Package = Convert.ToBase64String(release.Package ?? new byte[0]),
        ReleaseDate = release.Date
      };

      return new DtoResponse<ReleaseInfoDto>(dto);
    }
    catch (Exception ex)
    {
      return new DtoResponse<ReleaseInfoDto>(ex);
    }
  }

  [HttpPost]
  public DtoResponse<LicenseeInfoDto> LicenseeInfo([FromQuery] Guid licenseeId, [FromQuery] Guid appId)
  {
    try
    {
      var licensee = _context.Licensee.Where(l => l.ID == licenseeId && l.ApplicationId == appId).FirstOrDefault() ??
        throw new LicenseeNotFoundException();

      LicenseeInfoDto dto = new LicenseeInfoDto()
      {
        ID = licensee.ID,
        Name = licensee.Name,
        ApplicationId = licensee.ApplicationId,
        ValidUntil = licensee.ValidUntil,
      };

      return new DtoResponse<LicenseeInfoDto>(dto);
    }
    catch (Exception ex)
    {
      return new DtoResponse<LicenseeInfoDto>(ex);
    }
  }
}