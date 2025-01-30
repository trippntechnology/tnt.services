namespace TNT.Services.Models.Exceptions;

/// <summary>
/// License not found exception
/// </summary>
public class LicenseeNotFoundException : Exception
{
  private const string DEFAULT_MESSAGE = "Licensee could not be found";

  /// <summary>
  /// Initialization constructor
  /// </summary>
  public LicenseeNotFoundException(string message = DEFAULT_MESSAGE) : base(message) { }
}
