namespace TNT.Services.Models.Request;

/// <summary>
/// Class representing an a credential associated with a application
/// </summary>
public class ApplicationCredential
{
  /// <summary>
  /// <see cref="Guid"/> associated with the application
  /// </summary>
  public Guid ID { get; set; }

  /// <summary>
  /// Secret (password) associated with the application
  /// </summary>
  public string Secret { get; set; } = string.Empty;
}
