using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TNT.Services.Models.Request;
using TNT.Services.Service.Data;

namespace TNT.Services.Service.Controllers
{
  [Route("api/[controller]/[action]")]
  [ApiController]
  public class AuthorizationController : ControllerBase
  {
    private readonly IConfiguration _configuration;
    private readonly ApplicationDbContext _context;
    private readonly DateTimeUtil _dateTimeUtil;
    private readonly GuidUtil _guidUtil;

    public AuthorizationController(IConfiguration config, ApplicationDbContext context, DateTimeUtil? dateTimeUtil = null, GuidUtil? guidUtil = null)
    {
      _configuration = config;
      _context = context;
      _dateTimeUtil = dateTimeUtil ?? new DateTimeUtil();
      _guidUtil = guidUtil ?? new GuidUtil();
    }

    [HttpPost]
    public ActionResult Authorize(ApplicationCredential credential)
    {
      if (!String.IsNullOrWhiteSpace(credential.Secret))
      {
        var application = _context.Application.Where(a => a.ID == credential.ID && a.Secret == credential.Secret).FirstOrDefault();

        if (application != null)
        {
          //create claims details based on the user information
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
            Expires = _dateTimeUtil.UtcNow.DateTime.AddMinutes(1),
            SigningCredentials = signIn
          };

          return Ok(new JwtSecurityTokenHandler().CreateEncodedJwt(descriptor));
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

  public class DateTimeUtil
  {
    public virtual DateTimeOffset UtcNow { get { return DateTimeOffset.UtcNow; } }
  }

  public class GuidUtil
  {
    public virtual Guid NewGuid() => Guid.NewGuid();
  }
}