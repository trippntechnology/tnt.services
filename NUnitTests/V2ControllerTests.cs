using Microsoft.EntityFrameworkCore;
using Moq;
using TNT.Services.Service.Controllers;
using TNT.Services.Service.Data;

namespace NUnitTests;

public class V2ControllerTests
{
  //private var baseUri = new Uri("https://localhost:5001/api");
  private Guid appId = Guid.Parse("fa1f92e1-4beb-4675-9290-0af2265909a2");
  private string secret = "2gHpq!TYt9xop65hSp";
  private Guid licenseeId = Guid.Parse("bc49bedc-80f9-4dca-b386-8739147f200d");

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
  public void Authenticate() { }

  //[Test]
  //public void ApplicationInfo()
  //{
  //  var mockContext = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
  //  mockContext.Setup(m => m.Application).Returns(GetDbSet(Applications));
    
  //  var sut = new V2Controller(mockContext.Object);

  //  var result = sut.ApplicationInfo(appId);
  //}
}
