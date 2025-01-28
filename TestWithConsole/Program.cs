using System.Text.Json;
using TNT.Services.Client;
using TNT.Services.Models;
using TNT.Services.Models.Dto;
using TNT.Services.Models.Request;

internal class Program
{
  private static void Main(string[] args)
  {
    var baseUri = new Uri("https://localhost:5001/api");
    var appId = Guid.Parse("fa1f92e1-4beb-4675-9290-0af2265909a2");
    var secret = "2gHpq!TYt9xop65hSp";
    var licenseeId = Guid.Parse("bc49bedc-80f9-4dca-b386-8739147f200d");
    var client = new Client(baseUri);

    Console.Write($"Calling GetJWT({appId}, {secret}) ... ");
    var response = client.Authorize(appId, secret);
    Console.WriteLine(response.IsSuccess ? "Success" : "Failed");
    var jwt = response.Data;

    if (jwt != null)
    {
      var authClient = new AuthenticatedClient(baseUri, jwt);

      Console.Write($"Calling ApplicationInfo({appId}) ... ");
      DtoResponse<ApplicationInfoDto> appInfoResponse = authClient.ApplicationInfo(appId);
      Console.WriteLine(appInfoResponse.IsSuccess ? "Success" : "Failed");
      var appInfo = appInfoResponse.Data;
      Console.WriteLine($"AppInfo:");
      Console.WriteLine(Serialize(appInfo));

      Console.Write($"Calling ReleaseInfo({appInfo?.ReleaseID ?? 0}) ... ");
      DtoResponse<ReleaseInfoDto> releaseResponse = authClient.ReleaseInfo(appInfo?.ReleaseID ?? 0);
      Console.WriteLine(releaseResponse.IsSuccess ? "Success" : "Failed");
      var releaseInfo = releaseResponse.Data;

      LicenseeRequest request = new LicenseeRequest() { ApplicationId = appId, LicenseeId = licenseeId };
      Console.Write($"Calling LicenseeInfo({request.ApplicationId}, {request.LicenseeId} ) ... ");
      DtoResponse<LicenseeInfoDto> licenseeResponse = authClient.LicenseeInfo(request.ApplicationId, request.LicenseeId);
      Console.WriteLine(licenseeResponse.IsSuccess ? "Success" : "Failed");
      var licenseeInfoDto = licenseeResponse.Data;
      Console.WriteLine("LicenseeInfoDto:");
      Console.WriteLine(Serialize(licenseeInfoDto));
    }
  }

  private static string Serialize(object? obj)
  {
    if (obj == null) return "";
    var options = new JsonSerializerOptions
    {
      PropertyNameCaseInsensitive = true,
      WriteIndented = true

    };
    return JsonSerializer.Serialize(obj, options);
  }
}