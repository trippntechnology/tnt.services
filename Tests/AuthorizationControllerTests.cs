using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using TNT.Services.Models.Request;
using TNT.Services.Service;
using TNT.Services.Service.Controllers;
using TNT.Services.Service.Data;
using TNT.Services.Service.Models.Entities;

namespace Tests
{
	[TestClass]
	public class AuthorizationControllerTests : ContextDependentTests
	{
		[TestMethod]
		public void AuthorizationController_Authorize_Invalid_Credential()
		{
			var mockContext = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
			mockContext.Setup(m => m.Application).Returns(GetDbSet(new List<Application>()
			{
				new Application() { ID = 1, Secret = "secret"}
			}));
			var mockConfig = new Mock<IConfiguration>();
			var sut = new AuthorizationController(mockConfig.Object, mockContext.Object);

			var result = sut.Authorize(null);
			Assert.AreEqual(400, (result as BadRequestResult).StatusCode);

			result = sut.Authorize(new ApplicationCredential());
			Assert.AreEqual(400, (result as BadRequestObjectResult).StatusCode);
		}

		[TestMethod]
		public void AuthorizationController_Authorize_Invalid_Application()
		{
			var mockContext = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
			mockContext.Setup(m => m.Application).Returns(GetDbSet(new List<Application>()
			{
				new Application() { ID = 1, Secret = "secret"}
			}));

			var mockConfig = new Mock<IConfiguration>();
			var sut = new AuthorizationController(mockConfig.Object, mockContext.Object);

			var result = sut.Authorize(new ApplicationCredential() { Secret = "secret" }) as BadRequestObjectResult;
			Assert.AreEqual(400, result.StatusCode);
			Assert.AreEqual("Invalid credentials", result.Value);

			result = sut.Authorize(new ApplicationCredential() { ID = 1, Secret = "foo" }) as BadRequestObjectResult;
			Assert.AreEqual(400, result.StatusCode);
			Assert.AreEqual("Invalid credentials", result.Value);
		}

		[TestMethod]
		public void AuthorizationController_Authorize_Valid()
		{
			var mockContext = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
			mockContext.Setup(m => m.Application).Returns(GetDbSet(new List<Application>()
			{
				new Application() { ID = 1, Name = "appName", Secret = "secret"}
			}));

			var mockConfig = new Mock<IConfiguration>();
			mockConfig.Setup(m => m[Setting.SUBJECT]).Returns("subject");
			mockConfig.Setup(m => m[Setting.KEY]).Returns("ThisIsAJWTKeyUsedToSignTheJWTItShouldBeSignificantlyLong");
			mockConfig.Setup(m => m[Setting.ISSUER]).Returns("issuer");
			mockConfig.Setup(m => m[Setting.AUDIENCE]).Returns("audience");

			var dt = DateTime.Parse("1/28/2020 8:10:55 AM");
			var mockDateTime = new Mock<DateTimeUtil>();
			mockDateTime.Setup(m => m.UtcNow).Returns(dt);

			var guid = Guid.Parse("595CA573-2EE6-4583-BD8C-C48584F60A86");
			var mockGuidUtil = new Mock<GuidUtil>();
			mockGuidUtil.Setup(m => m.NewGuid()).Returns(guid);

			var sut = new AuthorizationController(mockConfig.Object, mockContext.Object, mockDateTime.Object, mockGuidUtil.Object);

			var result = sut.Authorize(new ApplicationCredential { ID = 1, Secret = "secret" }) as OkObjectResult;
			Assert.AreEqual(200, result.StatusCode);
			Assert.AreEqual("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJzdWJqZWN0IiwianRpIjoiNTk1Y2E1NzMtMmVlNi00NTgzLWJkOGMtYzQ4NTg0ZjYwYTg2IiwiaWF0IjoiMS8yOC8yMDIwIDg6MTA6NTUgQU0iLCJJZCI6IjEiLCJOYW1lIjoiYXBwTmFtZSIsImV4cCI6MTU4MDIyNDMxNSwiaXNzIjoiaXNzdWVyIiwiYXVkIjoiYXVkaWVuY2UifQ.zRzcngUND-VUFLnYFH1WTGFPZ8kdK5lmTPugv8U82mo", result.Value);
		}
	}
}