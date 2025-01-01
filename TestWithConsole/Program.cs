using TNT.Services.Client;
using TNT.Services.Models.Request;

internal class Program
{
  private static void Main(string[] args)
  {
    var appId = Guid.Parse("fa1f92e1-4beb-4675-9290-0af2265909a2");
    var licenseeId = Guid.Parse("bc49bedc-80f9-4dca-b386-8739147f200d");
    var client = new Client(new Uri("https://localhost:5001/api"));

    Console.Write($"Calling GetJWT({appId}, Secret1) ... ");
    var jwt = client.GetJWT(appId, "Secret1");
    Console.WriteLine(jwt.IsSuccess);

    if (jwt.Token != null)
    {
      Console.WriteLine($"Calling GetApplicationInfo({appId}) ... ");
      var appInfo = client.GetApplicationInfo(appId, jwt.Token);
      Console.WriteLine(appInfo?.IsSuccess);
      Console.WriteLine($"AppInfo: {appInfo}");

      Console.Write($"Calling GetRelease({appInfo?.ReleaseID ?? 0}) ... ");
      var release = client.GetRelease(appInfo?.ReleaseID ?? 0, jwt.Token);
      Console.WriteLine(release?.IsSuccess);

      LicenseeRequest request = new LicenseeRequest() { ApplicationId = appId, LicenseeId = licenseeId };
      Console.WriteLine($"Calling GetLicensee({request.ApplicationId}, {request.LicenseeId} )");
      Console.WriteLine("LicenseeResponse:");
      var licenseeResponse = client.GetLicensee(request, jwt.Token);
      Console.WriteLine(licenseeResponse.ToString());
    }
  }
}