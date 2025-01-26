namespace TNT.Services.Models;

/// <summary>
/// Response with Dto
/// </summary>
/// <typeparam name="T">Dto type</typeparam>
public class DtoResponse<T>
{
  /// <summary>
  /// Dto object
  /// </summary>
  public T? Data { get; private set; }

  /// <summary>
  /// Indicates the result of responding to a request
  /// </summary>
  public bool IsSuccess { get; private set; } = true;

  /// <summary>
  /// Message that can provide my details about the response
  /// </summary>
  public string Message { get; private set; } = string.Empty;

  /// <summary>
  /// Initialization constructor for a success response with data
  /// </summary>
  public DtoResponse(T Data)
  {
    this.Data = Data;
  }

  /// <summary>
  /// Constructor used when result is failure
  /// </summary>
  /// <param name="ex"></param>
  public DtoResponse(Exception ex)
  {
    this.IsSuccess = false;
    this.Message = $"{ex.GetType().Name}: {ex.Message}";
  }
}
