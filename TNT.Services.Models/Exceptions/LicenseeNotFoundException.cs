namespace TNT.Services.Models.Exceptions;

public class LicenseeNotFoundException : Exception
{
  private const string DEFAULT_MESSAGE = "Licensee could not be found";

  public LicenseeNotFoundException(string message = DEFAULT_MESSAGE) : base(message) { }
}
