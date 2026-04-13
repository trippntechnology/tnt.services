using System.Diagnostics.CodeAnalysis;
using TNT.Services.Service;

#nullable enable

namespace NUnitTests;

[ExcludeFromCodeCoverage]
public class ExtensionsTests
{
  [Test]
  public void Extensions_ToVersion_Invalid()
  {
    var defaultVersion = new Version();
    string? version = null;
    Assert.That(version?.ToVersion(), Is.Null);
    Assert.That("".ToVersion(), Is.EqualTo(defaultVersion));
    Assert.That("1".ToVersion(), Is.EqualTo(defaultVersion));
    Assert.That("1.1.1.1.1".ToVersion(), Is.EqualTo(defaultVersion));
  }

  [Test]
  public void Extensions_ToVersion_Valid()
  {
    var v1 = new Version("1.1");
    var v2 = new Version("1.1.1");
    var v3 = new Version("1.1.1.1");
    Assert.That("1.1".ToVersion(), Is.EqualTo(v1));
    Assert.That("1.1.1".ToVersion(), Is.EqualTo(v2));
    Assert.That("1.1.1.1".ToVersion(), Is.EqualTo(v3));
  }
}
