using RestSharp;
using TNT.Services.Models;
using TNT.Services.Models.Request;

namespace TNT.Services.Client;

/// <summary>
/// <see cref="BaseClient"/> used to authenicate
/// </summary>
public class Client(Uri baseUri) : BaseClient(baseUri)
{
  /// <summary>
  /// Called to get a <see cref="JWT"/> that can be used by the <see cref="AuthenticatedClient"/>
  /// </summary>
  /// <returns><see cref="JWT"/></returns>
  public DtoResponse<JWT> Authorize(Guid appId, string password)
  {
    ApplicationCredential appCredential = new ApplicationCredential() { ID = appId, Secret = password };
    RestRequest request = new RestRequest(Endpoint.Authorize.uri, Method.Post).AddJsonBody(appCredential);
    var response = client.ExecuteAsync(request).Result;

    try
    {
      var token = ProcessRestResponse<String>(response);
      return new DtoResponse<JWT>(new JWT(token));
    }
    catch (Exception ex)
    {
      return new DtoResponse<JWT>(ex);
    }
  }
}
