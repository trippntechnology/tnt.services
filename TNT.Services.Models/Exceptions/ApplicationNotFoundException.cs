namespace TNT.Services.Models.Exceptions;

public class ApplicationNotFoundException : Exception
{
  private const string DEFAULT_MESSAGE = "Application ID, {0} does not exist";

  public ApplicationNotFoundException(Guid id, string message = DEFAULT_MESSAGE) : base(string.Format(message, id)) { }
}
