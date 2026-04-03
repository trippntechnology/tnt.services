using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TNT.Services.Models.Exceptions;
using TNT.Services.Models.Request;
using TNT.Services.Models.Response;
using TNT.Services.Service.Data;

namespace TNT.Services.Service.Controllers;

/// <summary>
/// <summary>
/// API v1 controller for managing applications, releases, and licensees.
/// Provides endpoints for retrieving application information, release packages, and verifying licenses.
/// </summary>
/// <param name="dbContext">The application database context used for data access operations.</param>
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("api/[controller]/[action]")]
[ApiController]
public class V1Controller(ApplicationDbContext dbContext) : BaseController
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    /// <summary>
    /// Tests the connection to the API.
    /// </summary>
    /// <returns>A response message confirming successful connection.</returns>
    [HttpGet]
    public Response TestConnection()
    {
        return new Response() { Message = "Congratulations! You have successfully connected." };
    }

     /// <summary>
    /// Gets application information by application ID (deprecated).
    /// Use GetApplicationInfo instead. POST should not be used for read-only operations.
    /// Retrieves the application and its latest release details.
    /// </summary>
    /// <param name="applicationRequest">The request containing the application ID.</param>
    /// <returns>Application information including the latest release details.</returns>
    /// <exception cref="ApplicationNotFoundException">Thrown when the application is not found.</exception>
    /// <exception cref="ReleaseNotFoundException">Thrown when no release is found for the application.</exception>
    [Obsolete("Use GetApplicationInfo instead. POST should not be used for read-only operations.")]
    [HttpPost]
    public ApplicationInfo PostApplicationInfo(ApplicationRequest applicationRequest) => GetApplicationInfo(applicationRequest.ApplicationID);

    /// <summary>
    /// Gets application information by application ID.
    /// Retrieves the application and its latest release details.
    /// </summary>
    /// <param name="applicationId">The unique identifier of the application.</param>
    /// <returns>Application information including the latest release details.</returns>
    /// <exception cref="ApplicationNotFoundException">Thrown when the application is not found.</exception>
    /// <exception cref="ReleaseNotFoundException">Thrown when no release is found for the application.</exception>
    [HttpGet]
    public ApplicationInfo GetApplicationInfo([FromQuery] Guid applicationId)
    {
        ApplicationInfo? response = null;

        try
        {
            var application = _dbContext.Application.Where(a => a.ID == applicationId).FirstOrDefault();
            if (application == null) throw new ApplicationNotFoundException(applicationId);

            var release = _dbContext.Release.Where(r => r.ApplicationID == application.ID).OrderByDescending(r => r.Date).FirstOrDefault();
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

    /// <summary>
    /// Gets release information and package by release ID.
    /// </summary>
    /// <param name="releaseId">The unique identifier of the release.</param>
    /// <returns>Release information including the package as base64-encoded bytes.</returns>
    /// <exception cref="ReleaseNotFoundException">Thrown when the release is not found.</exception>
    [HttpGet]
    public ReleaseResponse GetRelease([FromQuery] int releaseId)
    {
        ReleaseResponse response;

        try
        {
            var release = _dbContext.Release.Where(r => r.ID == releaseId).FirstOrDefault() ??
            throw new ReleaseNotFoundException(releaseId, "Release ID, {0}, could not be found");

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

    /// <summary>
    /// Gets release information and package by release ID (deprecated).
    /// Use GetRelease instead. POST should not be used for read-only operations.
    /// </summary>
    /// <param name="releaseRequest">The release request containing the release ID.</param>
    /// <returns>Release information including the package as base64-encoded bytes.</returns>
    [Obsolete("Use GetRelease instead. POST should not be used for read-only operations.")]
    [HttpPost]
    public ReleaseResponse PostRelease(ReleaseRequest releaseRequest)
    {
        return GetRelease(releaseRequest.ReleaseId);
    }

    /// <summary>
    /// Verifies the validity of a license for a specific licensee and application.
    /// </summary>
    /// <param name="licenseeRequest">The request containing the licensee and application IDs.</param>
    /// <returns>License information including licensee details and validity date.</returns>
    /// <exception cref="LicenseeNotFoundException">Thrown when the licensee is not found for the specified application.</exception>
    [HttpPost]
    public LicenseeResponse PostVerifyLicense(LicenseeRequest licenseeRequest)
    {
        LicenseeResponse response;

        try
        {
            var licensee = _dbContext.Licensee.Where(l => l.ID == licenseeRequest.LicenseeId && l.ApplicationId == licenseeRequest.ApplicationId).FirstOrDefault() ??
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