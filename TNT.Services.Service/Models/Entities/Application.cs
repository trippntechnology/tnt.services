namespace TNT.Services.Service.Models.Entities;

/// <summary>
/// Represents a registered application in the TNT Services system.
/// </summary>
/// <remarks>
/// Applications are authenticated using their ID and Secret credentials.
/// Each application can generate JWT tokens for accessing protected API endpoints
/// and may store encryption credentials (CipherIV and CipherKey) for data encryption operations.
/// </remarks>
public class Application
{
  /// <summary>
  /// Gets or sets the unique identifier for the application.
  /// </summary>
  public Guid ID { get; set; }

  /// <summary>
  /// Gets or sets the display name of the application.
  /// </summary>
  /// <remarks>
  /// This is a required, human-readable identifier for the application.
  /// </remarks>
  public required string Name { get; set; }

  /// <summary>
  /// Gets or sets the secret credential used for authenticating the application.
  /// </summary>
  /// <remarks>
  /// This is a required value that should be kept confidential.
  /// Used alongside the ID to validate application credentials during authorization.
  /// </remarks>
  public required string Secret { get; set; }

  /// <summary>
  /// Gets or sets the cipher initialization vector (IV) as a base64-encoded string.
  /// </summary>
  /// <remarks>
  /// The IV is used with the cipher key for encryption and decryption operations.
  /// Null if encryption credentials have not been configured.
  /// </remarks>
  public string? CipherIV { get; set; }

  /// <summary>
  /// Gets or sets the cipher key as a base64-encoded string.
  /// </summary>
  /// <remarks>
  /// The key is used with the cipher IV for encryption and decryption operations.
  /// Null if encryption credentials have not been configured.
  /// </remarks>
  public string? CipherKey { get; set; }
}
