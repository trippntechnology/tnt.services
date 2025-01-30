namespace TNT.Services.Models.Exceptions;

/// <summary>
/// Application not found exception
/// </summary>
public class ApplicationNotFoundException : Exception
{
  private const string DEFAULT_MESSAGE = "Application ID, {0} does not exist";

  /// <summary>
  /// Initialization constructor
  /// </summary>
  public ApplicationNotFoundException(Guid id, string message = DEFAULT_MESSAGE) : base(string.Format(message, id)) { }
}
