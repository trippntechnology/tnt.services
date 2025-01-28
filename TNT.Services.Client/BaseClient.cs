using RestSharp;
using System.Text.Json;
using TNT.Commons;

namespace TNT.Services.Client;

/// <summary>
/// 
/// </summary>
/// <param name="baseUri"></param>
public abstract class BaseClient(Uri baseUri)
{
  /// <summary>
  /// 
  /// </summary>
  protected readonly IRestClient client = new RestClient(baseUri);

  /// <summary>
  /// 
  /// </summary>
  protected readonly Uri baseUri = baseUri;

  /// <summary>
  /// 
  /// </summary>
  /// <typeparam name="TOut"></typeparam>
  /// <param name="response"></param>
  /// <returns></returns>
  /// <exception cref="Exception"></exception>
  protected TOut ProcessRestResponse<TOut>(RestResponse? response)
  {
    if (response == null) throw new Exception("Response was null");
    if (response.Content == null) throw new Exception("Content was null");

    if (!response.IsSuccessful)
    {
      response.ErrorException?.also(it => throw it);
    }

    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    return JsonSerializer.Deserialize<TOut>(response.Content, options) ?? throw new Exception("Deserialization resulted in a null value");
  }
}
