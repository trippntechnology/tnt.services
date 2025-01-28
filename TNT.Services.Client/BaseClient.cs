using RestSharp;
using System.Text.Json;
using TNT.Commons;

namespace TNT.Services.Client;

/// <summary>
/// Base client
/// </summary>
public abstract class BaseClient(Uri baseUri)
{
  /// <summary>
  /// <see cref="IRestClient"/> used by subclasses
  /// </summary>
  protected readonly IRestClient client = new RestClient(baseUri);

  /// <summary>
  /// Base <see cref="Uri"/>
  /// </summary>
  protected readonly Uri baseUri = baseUri;

  /// <summary>
  /// Deserializes the content from a <see cref="RestResponse"/>
  /// </summary>
  /// <typeparam name="TOut">Type of object that content represents</typeparam>
  /// <returns>Object of type <typeparamref name="TOut"/> if exists, null otherwise</returns>
  /// <exception cref="Exception">Thrown when unable to deserialize <paramref name="response"/></exception>
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
