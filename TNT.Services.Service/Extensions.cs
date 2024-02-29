namespace TNT.Services.Service
{
  public static class Extensions
  {
    public static Version ToVersion(this string value)
    {
      if (Version.TryParse(value, out Version? version))
      {
        return version;
      }
      else
      {
        return new Version();
      }
    }
  }
}
