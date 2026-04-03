using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TNT.Services.Models;
using TNT.Services.Models.Dto;
using TNT.Services.Models.Exceptions;
using TNT.Services.Service.Data;
using TNT.Services.Service.Models.Entities;

namespace TNT.Services.Service.Controllers;

/// <summary>
/// API controller for version 2 endpoints providing application, release, licensee, and analytics information.
/// </summary>
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("api/[controller]/[action]")]
[ApiController]
public class V2Controller(ApplicationDbContext dbContext) : BaseController
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    /// <summary>
    /// Tests the API connection and authentication.
    /// </summary>
    [HttpGet]
    public DtoResponse<string> Test()
    {
        return new DtoResponse<string>("Congratulations! You have successfully connected.");
    }

    /// <summary>
    /// Retrieves application and release information.
    /// </summary>
    [HttpGet]
    public DtoResponse<ApplicationInfoDto> ApplicationInfo([FromQuery] Guid applicationId)
    {
        try
        {
            var application = _dbContext.Application.Where(a => a.ID == applicationId).FirstOrDefault();
            if (application == null) throw new ApplicationNotFoundException(applicationId);

            var release = _dbContext.Release.Where(r => r.ApplicationID == application.ID).OrderByDescending(r => r.Date).FirstOrDefault();
            if (release == null) throw new ReleaseNotFoundException(application.ID);

            ApplicationInfoDto dto = new ApplicationInfoDto()
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

    /// <summary>
    /// Retrieves release information with package encoded as Base64.
    /// </summary>
    [HttpGet]
    public DtoResponse<ReleaseInfoDto> ReleaseInfo([FromQuery] int releaseId)
    {
        try
        {
            var release = _dbContext.Release.Where(r => r.ID == releaseId).FirstOrDefault() ??
            throw new ReleaseNotFoundException(releaseId, "Release ID, {0}, could not be found");

            ReleaseInfoDto dto = new ReleaseInfoDto()
            {
                Id = release.ID,
                FileName = release.FileName,
                Package = Convert.ToBase64String(release.Package),
                ReleaseDate = release.Date
            };

            return new DtoResponse<ReleaseInfoDto>(dto);
        }
        catch (Exception ex)
        {
            return new DtoResponse<ReleaseInfoDto>(ex);
        }
    }

    /// <summary>
    /// Retrieves licensee information.
    /// </summary>
    [HttpGet]
    public DtoResponse<LicenseeInfoDto> LicenseeInfo([FromQuery] Guid licenseeId, [FromQuery] Guid appId)
    {
        try
        {
            var licensee = _dbContext.Licensee.Where(l => l.ID == licenseeId && l.ApplicationId == appId).FirstOrDefault() ??
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

    /// <summary>
    /// Adds a new analytics event.
    /// </summary>
    [HttpPost]
    public DtoResponse<int> AddAnalytic([FromBody] AnalyticDto dto)
    {
        try
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            var analytic = new Analytic()
            {
                Timestamp = dto.Timestamp,
                EventType = dto.EventType,
                Metadata = System.Text.Json.JsonSerializer.Serialize(dto.Metadata)
            };

            _dbContext.Analytics.Add(analytic);
            _dbContext.SaveChanges();

            return new DtoResponse<int>(analytic.Id);
        }
        catch (Exception ex)
        {
            return new DtoResponse<int>(ex);
        }
    }
}