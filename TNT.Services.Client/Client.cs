﻿using Newtonsoft.Json;
using RestSharp;
using TNT.Commons;
using TNT.Services.Models;
using TNT.Services.Models.Request;
using TNT.Services.Models.Response;

namespace TNT.Services.Client;

public class Client
{
  private const string GET_APPLICATION_INFO_ENDPOINT = "/v1/GetApplicationInfo";
  private const string GET_RELEASE_ENDPOINT = "/v1/GetRelease/";
  private const string POST_AUTHORIZE_ENDPOINT = "/Authorization/PostAuthorize";
  private const string POST_VERIFY_LICENSSEE = "/v1/PostVerifyLicense";

  protected IRestClient apiClient;

  public Client(Uri apiUri)
  {
    apiClient = new RestClient(apiUri);
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


  public ApplicationInfo GetApplicationInfo(Guid appId, JWT jwt)
  {
    ApplicationRequest appRequest = new ApplicationRequest() { ApplicationID = appId };
    RestRequest request = new RestRequest(GET_APPLICATION_INFO_ENDPOINT, Method.Get)
      .AddHeader("Authorization", jwt.ToAuthToken)
      .AddQueryParameter("applicationId", appId.ToString());
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

  public Task<ApplicationInfo> GetApplicationInfoAsync(Guid appid, JWT jwt)
  {
    return Task.Run(() => { return GetApplicationInfo(appid, jwt); });
  }

  public ReleaseResponse GetRelease(int releaseId, JWT jwt)
  {
    ReleaseRequest releaseRequest = new ReleaseRequest() { ReleaseId = releaseId };
    RestRequest request = new RestRequest(GET_RELEASE_ENDPOINT, Method.Get)
      .AddHeader("Authorization", jwt.ToAuthToken)
      .AddQueryParameter("ReleaseId", releaseId.ToString());
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

  public LicenseeResponse GetLicensee(LicenseeRequest licenseeRequest, JWT jwt)
  {
    RestRequest request = new RestRequest(POST_VERIFY_LICENSSEE, Method.Post)
      .AddHeader("Authorization", jwt.ToAuthToken)
      .AddBody(licenseeRequest);
    RestResponse response = apiClient.ExecuteAsync(request).Result;

    try
    {
      return ProcessRestResponse<LicenseeResponse>(response);
    }
    catch (Exception ex)
    {
      return new LicenseeResponse(ex);
    }
  }

  public JWTResponse GetJWT(Guid appId, string password)
  {
    ApplicationCredential appCredential = new ApplicationCredential() { ID = appId, Secret = password };
    RestRequest request = new RestRequest(POST_AUTHORIZE_ENDPOINT, Method.Post).AddJsonBody(appCredential);
    var response = apiClient.ExecuteAsync(request).Result;

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
