using RestSharp;
using TNT.Services.Models;
using TNT.Services.Models.Dto;
using TNT.Services.Models.Request;

namespace TNT.Services.Client;

/// <summary>
/// 
/// </summary>
/// <param name="baseUri"></param>
/// <param name="jwt"></param>
public class AuthenticatedClient(Uri baseUri, JWT jwt) : BaseClient(baseUri)
{
  private readonly JWT jwt = jwt;

  /// <summary>
  /// 
  /// </summary>
  /// <param name="appId"></param>
  /// <returns></returns>
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
  /// 
  /// </summary>
  /// <param name="releaseId"></param>
  /// <param name="jwt"></param>
  /// <returns></returns>
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
  /// 
  /// </summary>
  /// <param name="appId"></param>
  /// <param name="licenseeId"></param>
  /// <returns></returns>
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
