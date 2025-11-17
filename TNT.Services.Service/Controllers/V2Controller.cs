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
/// <remarks>
/// All endpoints require JWT Bearer token authentication. Uses the ApplicationDbContext for data access.
/// </remarks>
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

    /// <summary>
    /// Tests the API connection and authentication.
    /// </summary>
    /// <returns>A success message wrapped in a DtoResponse.</returns>
    [HttpGet]
    public DtoResponse<string> Test()
    {
        return new DtoResponse<string>("Congratulations! You have successfully connected.");
    }

    /// <summary>
    /// Retrieves information about a specific application and its latest release.
    /// </summary>
    /// <returns>Application and release information wrapped in a DtoResponse.</returns>
    [HttpGet]
    public DtoResponse<ApplicationInfoDto> ApplicationInfo([FromQuery] Guid applicationId)
    {
        try
        {
            var application = _context.Application.Where(a => a.ID == applicationId).FirstOrDefault();
            if (application == null) throw new ApplicationNotFoundException(applicationId);

            var release = _context.Release.Where(r => r.ApplicationID == application.ID).OrderByDescending(r => r.Date).FirstOrDefault();
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
    /// Retrieves information about a specific release package.
    /// </summary>
    /// <returns>Release information with the package encoded as Base64 wrapped in a DtoResponse.</returns>
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
    /// Retrieves information about a specific licensee.
    /// </summary>
    /// <returns>Licensee information wrapped in a DtoResponse.</returns>
    [HttpGet]
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

    /// <summary>
    /// Adds a new analytics event to the system.
    /// </summary>
    /// <returns>The ID of the newly created analytic record wrapped in a DtoResponse.</returns>
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

            _context.Analytics.Add(analytic);
            _context.SaveChanges();

            return new DtoResponse<int>(analytic.Id);
        }
        catch (Exception ex)
        {
            return new DtoResponse<int>(ex);
        }
    }
}