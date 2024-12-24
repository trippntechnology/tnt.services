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

      var result = sut.PostAuthorize(null);
      Assert.AreEqual(400, (result as BadRequestResult).StatusCode);

      result = sut.PostAuthorize(new ApplicationCredential());
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

      var result = sut.PostAuthorize(new ApplicationCredential() { Secret = "secret" }) as BadRequestObjectResult;
      Assert.AreEqual(400, result.StatusCode);
      Assert.AreEqual("Invalid credentials", result.Value);

      result = sut.PostAuthorize(new ApplicationCredential() { ID = 1, Secret = "foo" }) as BadRequestObjectResult;
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

      var dt = DateTime.Parse("1/28/2030 8:10:55 AM");
      var mockDateTime = new Mock<DateTimeUtil>();
      mockDateTime.Setup(m => m.UtcNow).Returns(dt);

      var guid = Guid.Parse("595CA573-2EE6-4583-BD8C-C48584F60A86");
      var mockGuidUtil = new Mock<GuidUtil>();
      mockGuidUtil.Setup(m => m.NewGuid()).Returns(guid);

      var sut = new AuthorizationController(mockConfig.Object, mockContext.Object, mockDateTime.Object, mockGuidUtil.Object);

      var result = sut.PostAuthorize(new ApplicationCredential { ID = 1, Secret = "secret" }) as OkObjectResult;
      Assert.AreEqual(200, result.StatusCode);
      Console.WriteLine($"SMT: {result.Value}");
      var values = result.Value.ToString().Split('.');
      Assert.AreEqual("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9", values[0]);
      //Assert.AreEqual("eyJzdWIiOiJzdWJqZWN0IiwianRpIjoiNTk1Y2E1NzMtMmVlNi00NTgzLWJkOGMtYzQ4NTg0ZjYwYTg2IiwiaWF0IjoxNzA5MjIzNzA3LCJJZCI6IjEiLCJOYW1lIjoiYXBwTmFtZSIsIm5iZiI6MTcwOTIyMzcwNywiZXhwIjoxODk1ODQzNTE1LCJpc3MiOiJpc3N1ZXIiLCJhdWQiOiJhdWRpZW5jZSJ9", values[1]);
      //Assert.AreEqual("JgHDnscnswLBtK_kHu0ED_am4RRxv8ZiWagIai1rtRE", values[2]);
      //Assert.AreEqual("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJzdWJqZWN0IiwianRpIjoiNTk1Y2E1NzMtMmVlNi00NTgzLWJkOGMtYzQ4NTg0ZjYwYTg2IiwiSWQiOiIxIiwiTmFtZSI6ImFwcE5hbWUiLCJuYmYiOjE3MDkyMjI0NDksImV4cCI6MTg5NTg0MzUxNSwiaWF0IjoxNzA5MjIyNDQ5LCJpc3MiOiJpc3N1ZXIiLCJhdWQiOiJhdWRpZW5jZSJ9.DhB0gCt-ZVZr51r5tvXjaBZCf9TDnMm1m0CLVy1yPEU", result.Value);
    }
  }
}