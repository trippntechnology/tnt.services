using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using TNT.Services.Models.Request;
using TNT.Services.Service;
using TNT.Services.Service.Controllers;
using TNT.Services.Service.Data;
using TNT.Services.Service.Models.Entities;

namespace NUnitTests;

public class AuthorizationControllerTests : ContextDependentTests
{
  private Guid appId = Guid.NewGuid();

  [Test]
  public void AuthorizationController_Authorize_Invalid_Credential()
  {
    var mockContext = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
    mockContext.Setup(m => m.Application).Returns(GetDbSet(new List<Application>()
    {
      new Application() { ID = appId, Secret = "secret"}
    }));
    var mockConfig = new Mock<IConfiguration>();
    var sut = new AuthorizationController(mockConfig.Object, mockContext.Object);

    var result = sut.Authorize(new ApplicationCredential());
    Assert.That((result as BadRequestObjectResult)?.StatusCode, Is.EqualTo(400));
  }

  [Test]
  public void AuthorizationController_Authorize_Invalid_Application()
  {
    var mockContext = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
    mockContext.Setup(m => m.Application).Returns(GetDbSet(new List<Application>()
    {
      new Application() { ID = appId, Secret = "secret"}
    }));

    var mockConfig = new Mock<IConfiguration>();
    var sut = new AuthorizationController(mockConfig.Object, mockContext.Object);

    var result = sut.Authorize(new ApplicationCredential() { Secret = "secret" }) as BadRequestObjectResult;
    Assert.That(result?.StatusCode, Is.EqualTo(400));
    Assert.That(result.Value, Is.EqualTo("Invalid credentials"));

    result = sut.Authorize(new ApplicationCredential() { ID = appId, Secret = "foo" }) as BadRequestObjectResult;
    Assert.That(result?.StatusCode, Is.EqualTo(400));
    Assert.That(result.Value, Is.EqualTo("Invalid credentials"));
  }

  [Test]
  public void AuthorizationController_Authorize_Valid()
  {
    var mockContext = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
    mockContext.Setup(m => m.Application).Returns(GetDbSet(new List<Application>()
    {
      new Application() { ID = appId, Name = "appName", Secret = "secret"}
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

    var result = sut.Authorize(new ApplicationCredential { ID = appId, Secret = "secret" }) as OkObjectResult;
    Assert.That(result?.StatusCode, Is.EqualTo(200));
    Console.WriteLine($"SMT: {result.Value}");
    var values = result?.Value?.ToString()?.Split('.') ?? new string[0];
    Assert.That(values[0], Is.EqualTo("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9"));
    //Assert.AreEqual("eyJzdWIiOiJzdWJqZWN0IiwianRpIjoiNTk1Y2E1NzMtMmVlNi00NTgzLWJkOGMtYzQ4NTg0ZjYwYTg2IiwiaWF0IjoxNzA5MjIzNzA3LCJJZCI6IjEiLCJOYW1lIjoiYXBwTmFtZSIsIm5iZiI6MTcwOTIyMzcwNywiZXhwIjoxODk1ODQzNTE1LCJpc3MiOiJpc3N1ZXIiLCJhdWQiOiJhdWRpZW5jZSJ9", values[1]);
    //Assert.AreEqual("JgHDnscnswLBtK_kHu0ED_am4RRxv8ZiWagIai1rtRE", values[2]);
    //Assert.AreEqual("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJzdWJqZWN0IiwianRpIjoiNTk1Y2E1NzMtMmVlNi00NTgzLWJkOGMtYzQ4NTg0ZjYwYTg2IiwiSWQiOiIxIiwiTmFtZSI6ImFwcE5hbWUiLCJuYmYiOjE3MDkyMjI0NDksImV4cCI6MTg5NTg0MzUxNSwiaWF0IjoxNzA5MjIyNDQ5LCJpc3MiOiJpc3N1ZXIiLCJhdWQiOiJhdWRpZW5jZSJ9.DhB0gCt-ZVZr51r5tvXjaBZCf9TDnMm1m0CLVy1yPEU", result.Value);
  }
}