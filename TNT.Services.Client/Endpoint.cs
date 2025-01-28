namespace TNT.Services.Client;

internal sealed class Endpoint(Uri uri)
{
  public Uri uri { get; } = uri;

  public static Endpoint Authorize => new Endpoint(new Uri("/Authorization/Authorize", UriKind.Relative));
  public static Endpoint ApplicationInfo => new Endpoint(new Uri("/v2/ApplicationInfo", UriKind.Relative));
  public static Endpoint ReleaseInfo => new Endpoint(new Uri("/v2/ReleaseInfo", UriKind.Relative));
  public static Endpoint LicenseeInfo => new Endpoint(new Uri("v2/LicenseeInfo", UriKind.Relative));
}
