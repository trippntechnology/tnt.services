namespace TNT.Services.Models.Exceptions;

/// <summary>
/// Release not found exception
/// </summary>
public class ReleaseNotFoundException : Exception
{
  private const string DEFAULT_MESSAGE = "Release associated with application ID, {0}, could not be found";


  /// <summary>
  /// Initialization constructor
  /// </summary>
  public ReleaseNotFoundException(int id, string message = DEFAULT_MESSAGE) : base(string.Format(message, id)) { }

  /// <summary>
  /// Initialization constructor
  /// </summary>
  public ReleaseNotFoundException(Guid appId, string message = DEFAULT_MESSAGE) : base(string.Format(message, appId)) { }
}
