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
	[Route("api/[controller]")]
	[ApiController]
	public class TokenController : ControllerBase
	{
		public IConfiguration _configuration;
		private readonly ApplicationDbContext _context;

		public TokenController(IConfiguration config, ApplicationDbContext context)
		{
			_configuration = config;
			_context = context;
		}

		[HttpPost]
		public ActionResult Post(ApplicatoinCredential credential)
		{

			if (credential != null && credential.Password != null)
			{
				var application = _context.Application.Where(a => a.ID == credential.ApplicationId && a.Password == credential.Password).FirstOrDefault();

				if (application != null)
				{
					//create claims details based on the user information
					var claims = new[] {
										new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
										new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
										new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
										new Claim("Id", application.ID.ToString()),
										new Claim("Name", application.Name)
									 };

					var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

					var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

					var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], claims, expires: DateTime.UtcNow.AddMinutes(1), signingCredentials: signIn);

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
}