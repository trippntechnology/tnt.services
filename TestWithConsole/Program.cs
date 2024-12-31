using TNT.Services.Client;
using TNT.Services.Models.Request;

internal class Program
{
  private static void Main(string[] args)
  {
    var client = new Client(new Uri("https://localhost:5001/api"));

    Console.Write($"Calling GetJWT(1, Secret1) ... ");
    var jwt = client.GetJWT(1, "Secret1");
    Console.WriteLine(jwt.IsSuccess);

    if (jwt.Token != null)
    {
      Console.Write($"Calling GetApplicationInfo(1) ... ");
      var appInfo = client.GetApplicationInfo(1, jwt.Token);
      Console.WriteLine(appInfo?.IsSuccess);
      Console.WriteLine($"AppInfo: {appInfo}");

      Console.Write($"Calling GetRelease(1) ... ");
      var release = client.GetRelease(1, jwt.Token);
      Console.WriteLine(release?.IsSuccess);

      LicenseeRequest request = new LicenseeRequest() { ApplicationId = 1, LicenseeId = Guid.Parse("fe94e00e-16cb-41e7-9c11-69f7f719819e") };
      Console.WriteLine($"Calling GetLicensee({request.ApplicationId}, {request.LicenseeId} )");
      Console.WriteLine("LicenseeResponse:");
      var licenseeResponse = client.GetLicensee(request, jwt.Token);
      Console.WriteLine(licenseeResponse.ToString());
    }
  }
}