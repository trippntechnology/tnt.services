using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TNT.Services.Models.Request;
using TNT.Services.Service.Data;

namespace TNT.Services.Service.Controllers;

/// <summary>
/// Controller for handling application authorization and JWT token generation.
/// </summary>
/// <param name="config">The configuration containing JWT settings.</param>
/// <param name="context">The application database context.</param>
/// <param name="dateTimeUtil">Optional utility for date/time operations; if null, a new instance is created.</param>
/// <param name="guidUtil">Optional utility for GUID generation; if null, a new instance is created.</param>
[Route("api/[controller]/[action]")]
[ApiController]
public class AuthorizationController(IConfiguration config, ApplicationDbContext context, DateTimeUtil? dateTimeUtil = null, GuidUtil? guidUtil = null) : ControllerBase
{
    private readonly IConfiguration _configuration = config;
    private readonly ApplicationDbContext _context = context;
    private readonly DateTimeUtil _dateTimeUtil = dateTimeUtil ?? new DateTimeUtil();
    private readonly GuidUtil _guidUtil = guidUtil ?? new GuidUtil();

    /// <summary>
    /// Authorizes an application by validating credentials and generating a JWT token.
    /// </summary>
    /// <param name="credential">The application credentials containing ID and Secret.</param>
    /// <returns>An OkObjectResult containing the JWT token if credentials are valid, otherwise a BadRequestResult.</returns>
    [HttpPost]
    public ActionResult Authorize(ApplicationCredential credential)
    {
        if (!String.IsNullOrWhiteSpace(credential.Secret))
        {
            var application = _context.Application.Where(a => a.ID == credential.ID && a.Secret == credential.Secret).FirstOrDefault();

            if (application != null)
            {
                // Create claims details based on the application information.
                var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, _configuration[Setting.SUBJECT]!),
                new Claim(JwtRegisteredClaimNames.Jti, _guidUtil.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, _dateTimeUtil.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer),
                new Claim("Id", application.ID.ToString()),
                new Claim("Name", application.Name)
               };

                SymmetricSecurityKey? key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration[Setting.KEY]!));

                SigningCredentials signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
                {
                    Issuer = _configuration[Setting.ISSUER],
                    Audience = _configuration[Setting.AUDIENCE],
                    Subject = new ClaimsIdentity(claims),
                    NotBefore = _dateTimeUtil.UtcNow.UtcDateTime,
                    Expires = _dateTimeUtil.UtcNow.UtcDateTime.AddMinutes(1),
                    SigningCredentials = signIn
                };

                var token = new JwtSecurityTokenHandler().CreateEncodedJwt(descriptor);
                return Ok(token);
            }
            else
            {
                return BadRequest("Invalid credentials");
            }
        }
        else
        {
            return BadRequest();
        }
    }
}

/// <summary>
/// Utility class for date/time operations.
/// </summary>
public class DateTimeUtil
{
    /// <summary>
    /// Gets the current UTC date and time.
    /// </summary>
    public virtual DateTimeOffset UtcNow { get { return DateTimeOffset.UtcNow; } }
}

/// <summary>
/// Utility class for GUID generation.
/// </summary>
public class GuidUtil
{
    /// <summary>
    /// Generates a new GUID.
    /// </summary>
    /// <returns>A new GUID.</returns>
    public virtual Guid NewGuid() => Guid.NewGuid();
}