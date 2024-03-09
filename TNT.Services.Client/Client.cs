using Newtonsoft.Json;
using RestSharp;
using System.Net;
using TNT.Commons;
using TNT.Services.Models;
using TNT.Services.Models.Request;
using TNT.Services.Models.Response;

namespace TNT.Services.Client
{
  public class Client
  {
    private const string APPLICATION_INFO_ENDPOINT = "/GetApplicationInfo/";
    private const string RELEASE_ENDPOINT = "/GetRelease/";
    private const string AUTHORIZE = "/Authorize/";

    protected IRestClient apiClient;
    protected IRestClient tokenClient;

    public Client(Uri apiUri, Uri tokenUri)
    {
      // Added to solve issue on Windows 7
      ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls13 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
      apiClient = new RestClient(apiUri);
      tokenClient = new RestClient(tokenUri);
    }

    private TOut ProcessRestResponse<TOut>(RestResponse? response)
    {
      if (response == null) throw new Exception("Response was null");
      if (response.Content == null) throw new Exception("Content was null");

      if (!response.IsSuccessful)
      {
        response.ErrorException?.also(it => throw it);
      }

      return JsonConvert.DeserializeObject<TOut>(response.Content) ?? throw new Exception("Deserialization resulted in a null value");
    }


    public ApplicationInfo GetApplicationInfo(int appId, JWT jwt)
    {
      ApplicationRequest appRequest = new ApplicationRequest() { ApplicationID = appId };
      RestRequest request = new RestRequest(APPLICATION_INFO_ENDPOINT, Method.Post)
        .AddHeader("Authorization", jwt.ToAuthToken)
        .AddJsonBody(appRequest);
      RestResponse response = apiClient.ExecuteAsync(request).Result;

      try
      {
        return ProcessRestResponse<ApplicationInfo>(response);
      }
      catch (Exception ex)
      {
        return new ApplicationInfo(ex);
      }
    }

    public Task<ApplicationInfo> GetApplicationInfoAsync(int appid, JWT jwt)
    {
      return Task.Run(() => { return GetApplicationInfo(appid, jwt); });
    }

    public ReleaseResponse GetRelease(int releaseId, JWT jwt)
    {
      ReleaseRequest releaseRequest = new ReleaseRequest() { ReleaseId = releaseId };
      RestRequest request = new RestRequest(RELEASE_ENDPOINT, Method.Post)
        .AddHeader("Authorization", jwt.ToAuthToken)
        .AddJsonBody(releaseRequest);
      var response = apiClient?.ExecuteAsync(request).Result;

      try
      {
        return ProcessRestResponse<ReleaseResponse>(response);
      }
      catch (Exception ex)
      {
        return new ReleaseResponse(ex);
      }
    }

    public Task<ReleaseResponse> GetReleaseAsync(int releaseId, JWT jwt)
    {
      return Task.Run(() => { return GetRelease(releaseId, jwt); });
    }

    public JWTResponse GetJWT(int appId, string password)
    {
      ApplicationCredential appCredential = new ApplicationCredential() { ID = appId, Secret = password };
      RestRequest request = new RestRequest(AUTHORIZE, Method.Post).AddJsonBody(appCredential);
      var response = tokenClient.ExecuteAsync(request).Result;

      try
      {
        var token = ProcessRestResponse<String>(response) ?? throw new Exception("Token is null");
        return new JWTResponse(token);
      }
      catch (Exception ex)
      {
        return new JWTResponse(ex);
      }
    }
  }
}
