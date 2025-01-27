﻿using Newtonsoft.Json;
using RestSharp;
using TNT.Commons;
using TNT.Services.Models;
using TNT.Services.Models.Request;
using TNT.Services.Models.Response;

namespace TNT.Services.Client;

/// <summary>
/// Client used to access the TNT.Services.Service
/// </summary>
public class Client
{
  private const string GET_APPLICATION_INFO_ENDPOINT = "/v1/GetApplicationInfo";
  private const string GET_RELEASE_ENDPOINT = "/v1/GetRelease/";
  private const string POST_AUTHORIZE_ENDPOINT = "/Authorization/PostAuthorize";
  private const string POST_VERIFY_LICENSSEE = "/v1/PostVerifyLicense";

  /// <summary>
  /// <see cref="IRestClient"/> used to access service. This client is instantiated with the <see cref="Uri"/> endpoint
  /// </summary>
  protected IRestClient apiClient;

  /// <summary>
  /// Initializes <see cref="apiClient"/> with a <see cref="Uri"/> to the endpoint
  /// </summary>
  /// <param name="apiUri"><see cref="Uri"/> specifying the api endpoint</param>
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

  /// <summary>
  /// Gets <see cref="ApplicationInfo"/> for a specified <paramref name="appId"/> and <paramref name="jwt"/>
  /// </summary>
  /// <param name="appId"><see cref="Guid"/> representing the app id of the application</param>
  /// <param name="jwt"><see cref="JWT"/> representing the bearer token</param>
  /// <returns><see cref="ApplicationInfo"/> for the requested application</returns>
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

  /// <summary>
  /// Gets <see cref="Task{ApplicationInfo}"/> for a specified <paramref name="appId"/> and <paramref name="jwt"/>
  /// </summary>
  /// <param name="appId"><see cref="Guid"/> representing the app id of the application</param>
  /// <param name="jwt"><see cref="JWT"/> representing the bearer token</param>
  /// <returns><see cref="Task{ApplicationInfo}"/> for the requested application</returns>
  public Task<ApplicationInfo> GetApplicationInfoAsync(Guid appId, JWT jwt)
  {
    return Task.Run(() => { return GetApplicationInfo(appId, jwt); });
  }

  /// <summary>
  /// Gets <see cref="ReleaseResponse"/> for a specified <paramref name="releaseId"/> and <paramref name="jwt"/>
  /// </summary>
  /// <param name="releaseId">release id</param>
  /// <param name="jwt"><see cref="JWT"/> representing the bearer token</param>
  /// <returns><see cref="ReleaseResponse"/> for the requested release</returns>
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

  /// <summary>
  /// Gets <see cref="Task{ReleaseResponse}"/> for a specified <paramref name="releaseId"/> and <paramref name="jwt"/>
  /// </summary>
  /// <param name="releaseId">release id</param>
  /// <param name="jwt"><see cref="JWT"/> representing the bearer token</param>
  /// <returns><see cref="Task{ReleaseResponse}"/> for the requested release</returns>
  public Task<ReleaseResponse> GetReleaseAsync(int releaseId, JWT jwt)
  {
    return Task.Run(() => { return GetRelease(releaseId, jwt); });
  }

  /// <summary>
  /// Gets <see cref="LicenseeResponse"/> for a specified <paramref name="licenseeRequest"/> and <paramref name="jwt"/>
  /// </summary>
  /// <param name="licenseeRequest"><see cref="LicenseeRequest"/> representing the request</param>
  /// <param name="jwt"><see cref="JWT"/> representing the bearer token</param>
  /// <returns><see cref="LicenseeResponse"/> for the requested license</returns>
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

  /// <summary>
  /// Used to return a <see cref="JWTResponse"/> containing the bearer token to access other methods.
  /// </summary>
  /// <param name="appId"><see cref="Guid"/> representing the app id</param>
  /// <param name="password">password associated with the app id</param>
  /// <returns><see cref="JWTResponse"/> if the credentials are valid</returns>
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
