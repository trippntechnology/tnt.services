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
    var appId = Guid.Parse("1b4f29eb-3543-413e-8b7c-30a2bd182fed");
    var secret = "secret";
    var licenseeId = Guid.Parse("b6ce5ba1-4632-40a9-8e1e-3bb1c5f426ed");
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

      // Log analytics examples
      Console.WriteLine("\n--- Logging Analytics Examples ---");

      // Example 1: Application Start Event
      Console.Write("Calling AddAnalytic with 'ApplicationStart' event ... ");
      var analyticsStartDto = new AnalyticDto("ApplicationStart")
      {
        Metadata = new Dictionary<string, string>
        {
          { "applicationId", appId.ToString() },
          { "os", "Windows" },
          { "osVersion", "10.0.19045" },
          { "appVersion", "2.3.1" }
        }
      };
      DtoResponse<int> analyticsStartResponse = authClient.AddAnalytic(analyticsStartDto);
      Console.WriteLine(analyticsStartResponse.IsSuccess ? "Success" : "Failed");
      if (analyticsStartResponse.IsSuccess)
      {
        Console.WriteLine($"  Created analytic record ID: {analyticsStartResponse.Data}");
      }

      // Example 2: Feature Usage Event
      Console.Write("Calling AddAnalytic with 'FeatureUsed' event ... ");
      var analyticsFeatureDto = new AnalyticDto("FeatureUsed")
      {
        Metadata = new Dictionary<string, string>
        {
          { "userId", licenseeId.ToString() },
          { "featureName", "ExportReport" },
          { "duration", "1250" },
          { "success", "true" },
          { "applicationVersion", "2.3.1" }
        }
      };
      DtoResponse<int> analyticsFeatureResponse = authClient.AddAnalytic(analyticsFeatureDto);
      Console.WriteLine(analyticsFeatureResponse.IsSuccess ? "Success" : "Failed");
      if (analyticsFeatureResponse.IsSuccess)
      {
        Console.WriteLine($"  Created analytic record ID: {analyticsFeatureResponse.Data}");
      }

      // Example 3: Error Event
      Console.Write("Calling AddAnalytic with 'Error' event ... ");
      var analyticsErrorDto = new AnalyticDto("Error")
      {
        Metadata = new Dictionary<string, string>
        {
          { "errorCode", "500" },
          { "errorMessage", "Database connection failed" },
          { "component", "DataService" },
          { "severity", "High" }
        }
      };
      DtoResponse<int> analyticsErrorResponse = authClient.AddAnalytic(analyticsErrorDto);
      Console.WriteLine(analyticsErrorResponse.IsSuccess ? "Success" : "Failed");
      if (analyticsErrorResponse.IsSuccess)
      {
        Console.WriteLine($"  Created analytic record ID: {analyticsErrorResponse.Data}");
      }
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