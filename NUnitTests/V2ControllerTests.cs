using Microsoft.EntityFrameworkCore;
using Moq;
using System.Diagnostics.CodeAnalysis;
using TNT.Services.Service.Controllers;
using TNT.Services.Service.Data;
using TNT.Services.Service.Models.Entities;

namespace NUnitTests;

[ExcludeFromCodeCoverage]
public class V2ControllerTests : ContextDependentTests
{
  [Test]
  public void Test()
  {
    var mockContext = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
    var sut = new V2Controller(mockContext.Object);
    var result = sut.Test();
    Assert.That(result.IsSuccess, Is.True);
    Assert.That(result.Data, Is.Not.Null);
    Assert.That(result.Data, Is.EqualTo("Congratulations! You have successfully connected."));
  }

  [Test]
  public void ApplicationInfo_Invalid_App_Id()
  {
    var mockContext = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
    var invalidAppId = Guid.Empty;
    mockContext.Setup(m => m.Application).Returns(GetDbSet(Applications));
    mockContext.Setup(m => m.Release).Returns(GetDbSet(Releases));

    var sut = new V2Controller(mockContext.Object);

    var result = sut.ApplicationInfo(invalidAppId);

    Assert.That(result.IsSuccess, Is.False);
    Assert.That(result.Message, Is.EqualTo($"ApplicationNotFoundException: Application ID, {invalidAppId} does not exist"));
  }

  [Test]
  public void ApplicationInfo_NoRelease()
  {
    var appId = Applications.First().ID;
    var mockContext = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
    mockContext.Setup(m => m.Application).Returns(GetDbSet(Applications));
    mockContext.Setup(m => m.Release).Returns(GetDbSet(new List<Release>()));

    var sut = new V2Controller(mockContext.Object);
    var result = sut.ApplicationInfo(appId);

    Assert.That(result.IsSuccess, Is.False);
    Assert.That(result.Message, Is.EqualTo($"ReleaseNotFoundException: Release associated with application ID, {appId}, could not be found"));
  }

  [Test]
  public void ApplicationInfo_Valid()
  {
    var appId = Applications.First().ID;
    var mockContext = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
    mockContext.Setup(m => m.Application).Returns(GetDbSet(Applications));
    mockContext.Setup(m => m.Release).Returns(GetDbSet(Releases));

    var sut = new V2Controller(mockContext.Object);

    var result = sut.ApplicationInfo(appId);
    Assert.That(result.IsSuccess, Is.True);
    Assert.That(string.IsNullOrWhiteSpace(result.Message), Is.True);

    var applicationInfo = result.Data;
    var releaseDate = Releases[0].Date ?? DateTime.Now;

    Assert.That(applicationInfo, Is.Not.Null);
    Assert.That(applicationInfo.ApplicationId, Is.EqualTo(Applications[0].ID));
    Assert.That(applicationInfo.Name, Is.EqualTo(Applications[0].Name));
    Assert.That(applicationInfo.ReleaseID, Is.EqualTo(Releases[0].ID));
    Assert.That(applicationInfo.ReleaseDate, Is.EqualTo(new DateTimeOffset(releaseDate)));
    Assert.That(applicationInfo.ReleaseVersion, Is.EqualTo(Releases[0].Version));
  }
}
