using Microsoft.EntityFrameworkCore;
using Moq;
using System.Diagnostics.CodeAnalysis;
using TNT.Services.Models.Request;
using TNT.Services.Service.Controllers;
using TNT.Services.Service.Data;
using TNT.Services.Service.Models.Entities;

namespace NUnitTests;

[ExcludeFromCodeCoverage]
public class V1ControllerTests : ContextDependentTests
{
  public Guid appId { get { return Applications.First().ID; } }

  [Test]
  public void V1Controller_TestConnection()
  {
    var mockContext = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
    var sut = new V1Controller(mockContext.Object);
    var result = sut.TestConnection();
    Assert.That(result.IsSuccess, Is.True);
    Assert.That(result.Message, Is.EqualTo("Congratulations! You have successfully connected."));
  }

  [Test]
  public void V1Controller_GetApplicationInfo_InvalidApplicationId()
  {
    var invalidAppId = Guid.NewGuid();
    var mockContext = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
    mockContext.Setup(m => m.Application).Returns(GetDbSet(Applications));
    mockContext.Setup(m => m.Release).Returns(GetDbSet(Releases));

    var sut = new V1Controller(mockContext.Object);

    var result = sut.GetApplicationInfo(invalidAppId);

    Assert.That(result.IsSuccess, Is.False);
    Assert.That(result.Message, Is.EqualTo($"ApplicationNotFoundException: Application ID, {invalidAppId} does not exist"));
  }

  [Test]
  public void V1Controller_GetApplicationInfo_NoRelease()
  {
    var mockContext = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
    mockContext.Setup(m => m.Application).Returns(GetDbSet(Applications));
    mockContext.Setup(m => m.Release).Returns(GetDbSet(new List<Release>()));

    var sut = new V1Controller(mockContext.Object);
    var result = sut.GetApplicationInfo(appId);

    Assert.That(result.IsSuccess, Is.False);
    Assert.That(result.Message, Is.EqualTo($"ReleaseNotFoundException: Release associated with application ID, {appId}, could not be found"));
  }

  [Test]
  public void V1Controller_GetApplicationInfo_Valid()
  {
    var mockContext = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
    mockContext.Setup(m => m.Application).Returns(GetDbSet(Applications));
    mockContext.Setup(m => m.Release).Returns(GetDbSet(Releases));

    var sut = new V1Controller(mockContext.Object);

    var result = sut.GetApplicationInfo(appId);

    Assert.That(result.Name, Is.EqualTo(Applications[0].Name));
    Assert.That(result.ReleaseVersion, Is.EqualTo(Releases[0].Version));
    Assert.That(result.ReleaseDate, Is.Not.Null);
    Assert.That(result.ReleaseID, Is.EqualTo(Releases[0].ID));
    Assert.That(result.IsSuccess, Is.True);
    Assert.That(string.IsNullOrWhiteSpace(result.Message), Is.True);
  }

  [Test]
  public void V1Controller_GetRelease_InvalidReleaseID()
  {
    var mockContext = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
    mockContext.Setup(m => m.Application).Returns(GetDbSet(Applications));
    mockContext.Setup(m => m.Release).Returns(GetDbSet(new List<Release>()));

    var sut = new V1Controller(mockContext.Object);
    var result = sut.PostRelease(new ReleaseRequest { ReleaseId = 1 });

    Assert.That(result.IsSuccess, Is.False);
    Assert.That(result.Message, Is.EqualTo("ReleaseNotFoundException: Release ID, 1, could not be found"));
  }

  [Test]
  public void V1Controller_GetRelease_Valid()
  {
    var mockContext = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
    mockContext.Setup(m => m.Application).Returns(GetDbSet(Applications));
    mockContext.Setup(m => m.Release).Returns(GetDbSet(Releases));

    var sut = new V1Controller(mockContext.Object);
    var result = sut.PostRelease(new ReleaseRequest { ReleaseId = 1 });

    Assert.That(result.IsSuccess, Is.True);
    Assert.That(result.ReleaseDate, Is.Not.Null);
    Assert.That(result.Package, Is.EqualTo(Convert.ToBase64String(Releases[0].Package)));
    Assert.That(result.FileName, Is.EqualTo(Releases[0].FileName));
  }
}
