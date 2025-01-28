using Microsoft.EntityFrameworkCore;
using Moq;
using System.Text;
using TNT.Services.Service.Models.Entities;

namespace NUnitTests;

public abstract class ContextDependentTests
{
  private List<Application>? _Applications = null;
  private List<Release>? _Releases = null;

  protected List<Application> Applications
  {
    get
    {
      if (_Applications == null)
      {
        _Applications = new List<Application>
        {
          new Application(){ ID = Guid.NewGuid(), Name= "Application1"},
          new Application(){ ID = Guid.NewGuid(), Name= "Application2"},
          new Application(){ ID = Guid.NewGuid(), Name= "Application3"},
        };
      }
      return _Applications;
    }
  }

  protected List<Release> Releases
  {
    get
    {
      if (_Releases == null)
      {
        _Releases = new List<Release>
        {
          new Release() {
            ApplicationID = Applications.First().ID,
            Date = DateTime.Now,
            FileName = "setup.exe",
            ID = 1,
            Package = Encoding.ASCII.GetBytes("Package"),
            Version = "1.2.3.4" }
        };
      }
      return _Releases;
    }
  }

  protected DbSet<T> GetDbSet<T>(List<T> results) where T : class
  {
    var data = new List<T>(results).AsQueryable();
    var mockDbSet = new Mock<DbSet<T>>();
    mockDbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(data.Provider);
    mockDbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(data.Expression);
    mockDbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(data.ElementType);
    mockDbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
    return mockDbSet.Object;
  }
}
