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

  [Test]
  public void ReleaseInfo_InvalidReleaseID()
  {
    var mockContext = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
    mockContext.Setup(m => m.Application).Returns(GetDbSet(Applications));
    mockContext.Setup(m => m.Release).Returns(GetDbSet(new List<Release>()));

    var sut = new V2Controller(mockContext.Object);
    var result = sut.ReleaseInfo(1);

    Assert.That(result.IsSuccess, Is.False);
    Assert.That(result.Message, Is.EqualTo("ReleaseNotFoundException: Release ID, 1, could not be found"));
  }

  [Test]
  public void ReleaseInfo_Valid()
  {
    var mockContext = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
    mockContext.Setup(m => m.Application).Returns(GetDbSet(Applications));
    mockContext.Setup(m => m.Release).Returns(GetDbSet(Releases));

    var sut = new V2Controller(mockContext.Object);
    var result = sut.ReleaseInfo(1);
    Assert.That(result.IsSuccess, Is.True);

    var releaseInfo = result.Data;
    DateTimeOffset dateTimeOffset = Releases[0].Date ?? DateTimeOffset.Now;

    Assert.That(releaseInfo, Is.Not.Null);
    Assert.That(releaseInfo.ReleaseDate, Is.EqualTo(dateTimeOffset));
    Assert.That(releaseInfo.Package, Is.EqualTo(Convert.ToBase64String(Releases[0].Package!)));
    Assert.That(releaseInfo.FileName, Is.EqualTo(Releases[0].FileName));
  }

  [Test]
  public void LicenseeInfo_Invalid()
  {
    var mockContext = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
    mockContext.Setup(m => m.Application).Returns(GetDbSet(Applications));
    mockContext.Setup(m => m.Licensee).Returns(GetDbSet(new List<Licensee>()));

    var sut = new V2Controller(mockContext.Object);
    var result = sut.LicenseeInfo(Licensees[0].ID, Applications[0].ID);

    Assert.That(result.IsSuccess, Is.False);
    Assert.That(result.Message, Is.EqualTo("LicenseeNotFoundException: Licensee could not be found"));
  }

  [Test]
  public void LicenseeInfo_Valid()
  {
    var mockContext = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
    mockContext.Setup(m => m.Licensee).Returns(GetDbSet(Licensees));

    var sut = new V2Controller(mockContext.Object);
    var result = sut.LicenseeInfo(Licensees[0].ID, Applications[0].ID);
    Assert.That(result.IsSuccess, Is.True);

    var releaseInfo = result.Data;

    Assert.That(releaseInfo, Is.Not.Null);
    Assert.That(releaseInfo.ID, Is.EqualTo(Licensees[0].ID));
    Assert.That(releaseInfo.Name, Is.EqualTo(Licensees[0].Name));
    Assert.That(releaseInfo.ApplicationId, Is.EqualTo(Applications[0].ID));
    Assert.That(releaseInfo.ValidUntil, Is.EqualTo(Licensees[0].ValidUntil));
  }
}
