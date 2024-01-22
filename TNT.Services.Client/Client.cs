using Newtonsoft.Json;
using RestSharp;
using System.Net;
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

    protected IRestClient? apiClient = null;
    protected IRestClient? tokenClient = null;

    public Client(Uri apiUri, Uri tokenUri)
    {
      // Added to solve issue on Windows 7
      ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
      apiClient = new RestClient(apiUri);
      tokenClient = new RestClient(tokenUri);
    }

    private TOut ProcessRestResponse<TOut>(RestResponse? response, Func<TOut> onError)
    {
      if (response == null) return onError();
      if (response.Content == null) return onError();

      if (response.IsSuccessful)
      {
        return JsonConvert.DeserializeObject<TOut>(response.Content) ?? onError();
      }
      else
      {
        return onError();
      }
    }

    public ApplicationInfo GetApplicationInfo(int appId, JWT jwt)
    {
      ApplicationRequest appRequest = new ApplicationRequest() { ApplicationID = appId };
      RestRequest request = new RestRequest(APPLICATION_INFO_ENDPOINT, Method.Post)
        .AddHeader("Authorization", jwt.ToAuthToken)
        .AddJsonBody(appRequest);
      var response = apiClient?.ExecuteAsync(request).Result;
      return ProcessRestResponse<ApplicationInfo>(response, () => new ApplicationInfo(new HttpRequestException()));
    }

    public Task<ApplicationInfo> GetApplicationInfoAsync(int appid, JWT jwt)
    {
      return Task.Run(() => { return GetApplicationInfo(appid, jwt); });
    }

    public ReleaseResponse GetRelease(int releaseId, JWT jwt)
    {
      var releaseRequest = new ReleaseRequest() { ReleaseId = releaseId };
      var request = new RestRequest(RELEASE_ENDPOINT, Method.Post)
        .AddHeader("Authorization", jwt.ToAuthToken)
        .AddJsonBody(releaseRequest);
      var response = apiClient?.ExecuteAsync(request).Result;
      return ProcessRestResponse<ReleaseResponse>(response, () => new ReleaseResponse(new HttpRequestException()));
    }

    public Task<ReleaseResponse> GetReleaseAsync(int releaseId, JWT jwt)
    {
      return Task.Run(() => { return GetRelease(releaseId, jwt); });
    }

    public JWTResponse GetJWT(int appId, string password)
    {
      var appCredential = new ApplicationCredential() { ID = appId, Secret = password };
      RestRequest request = new RestRequest(AUTHORIZE, Method.Post).AddJsonBody(appCredential);
      var response = tokenClient?.ExecuteAsync(request).Result;
      return ProcessRestResponse<JWTResponse>(response, () => new JWTResponse(new HttpRequestException()));
    }
  }
}
