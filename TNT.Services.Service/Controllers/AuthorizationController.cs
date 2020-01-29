using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
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

		public AuthorizationController(IConfiguration config, ApplicationDbContext context, DateTimeUtil dateTimeUtil = null, GuidUtil guidUtil = null)
		{
			_configuration = config;
			_context = context;
			_dateTimeUtil = dateTimeUtil ?? new DateTimeUtil();
			_guidUtil = guidUtil ?? new GuidUtil();
		}

		[HttpPost]
		public ActionResult Authorize(ApplicationCredential credential)
		{
			if (credential != null && credential.Secret != null)
			{
				var application = _context.Application.Where(a => a.ID == credential.ID && a.Secret == credential.Secret).FirstOrDefault();

				if (application != null)
				{
					//create claims details based on the user information
					var claims = new[] {
										new Claim(JwtRegisteredClaimNames.Sub, _configuration[Setting.SUBJECT]),
										new Claim(JwtRegisteredClaimNames.Jti, _guidUtil.NewGuid().ToString()),
										new Claim(JwtRegisteredClaimNames.Iat, _dateTimeUtil.UtcNow.ToString()),
										new Claim("Id", application.ID.ToString()),
										new Claim("Name", application.Name)
									 };

					var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration[Setting.KEY]));

					var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

					var token = new JwtSecurityToken(_configuration[Setting.ISSUER], _configuration[Setting.AUDIENCE], claims, expires: _dateTimeUtil.UtcNow.AddMinutes(1), signingCredentials: signIn);

					return Ok(new JwtSecurityTokenHandler().WriteToken(token));
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
		public virtual DateTime UtcNow { get { return DateTime.UtcNow; } }
	}

	public class GuidUtil
	{
		public virtual Guid NewGuid() => Guid.NewGuid();
	}
}