using RestSharp;
using TNT.Services.Models;
using TNT.Services.Models.Dto;
using TNT.Services.Models.Request;

namespace TNT.Services.Client;

/// <summary>
/// Client used to access endpoints that need to be authenticated. Authentication can be done using the <see cref="Client"/>
/// </summary>
public class AuthenticatedClient(Uri baseUri, JWT jwt) : BaseClient(baseUri)
{
  private readonly JWT jwt = jwt;

  /// <summary>
  /// Gets the application info for a specific <paramref name="appId"/>
  /// </summary>
  /// <returns><see cref="DtoResponse{ApplicationInfoDto}"/></returns>
  public DtoResponse<ApplicationInfoDto> ApplicationInfo(Guid appId)
  {
    ApplicationRequest appRequest = new ApplicationRequest() { ApplicationID = appId };
    RestRequest request = new RestRequest(Endpoint.ApplicationInfo.uri, Method.Get)
      .AddHeader("Authorization", jwt.ToAuthToken)
      .AddQueryParameter("applicationId", appId.ToString());
    RestResponse response = client.Execute(request);

    try
    {
      return ProcessRestResponse<DtoResponse<ApplicationInfoDto>>(response);
    }
    catch (Exception ex)
    {
      return new DtoResponse<ApplicationInfoDto>(ex);
    }
  }

  /// <summary>
  /// Gets the release info for a specific <paramref name="releaseId"/>
  /// </summary>
  /// <returns><see cref="DtoResponse{ReleaseInfoDto}"/></returns>
  public DtoResponse<ReleaseInfoDto> ReleaseInfo(int releaseId)
  {
    ReleaseRequest releaseRequest = new ReleaseRequest() { ReleaseId = releaseId };
    RestRequest request = new RestRequest(Endpoint.ReleaseInfo.uri, Method.Get)
      .AddHeader("Authorization", jwt.ToAuthToken)
      .AddQueryParameter("ReleaseId", releaseId.ToString());
    RestResponse response = client.Execute(request);

    try
    {
      return ProcessRestResponse<DtoResponse<ReleaseInfoDto>>(response);
    }
    catch (Exception ex)
    {
      return new DtoResponse<ReleaseInfoDto>(ex);
    }
  }

  /// <summary>
  /// Gets the licensee info for a specific <paramref name="licenseeId"/> and <paramref name="appId"/>
  /// </summary>
  /// <returns><see cref="DtoResponse{LicenseeInfoDto}"/></returns>
  public DtoResponse<LicenseeInfoDto> LicenseeInfo(Guid appId, Guid licenseeId)
  {
    RestRequest request = new RestRequest(Endpoint.LicenseeInfo.uri, Method.Get)
      .AddHeader("Authorization", jwt.ToAuthToken)
      .AddQueryParameter("appId", appId.ToString())
      .AddQueryParameter("licenseeId", licenseeId.ToString());
    RestResponse response = client.Execute(request);

    try
    {
      return ProcessRestResponse<DtoResponse<LicenseeInfoDto>>(response);
    }
    catch (Exception ex)
    {
      return new DtoResponse<LicenseeInfoDto>(ex);
    }
  }

}
