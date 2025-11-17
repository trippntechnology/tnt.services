namespace TNT.Services.Models.Dto;

/// <summary>
/// Data transfer object for submitting analytics events to the system.
/// </summary>
/// <remarks>
/// This DTO is used by clients to send analytics data to the server.
/// The <see cref="Timestamp"/> is automatically set to UTC now when the DTO is created.
/// </remarks>
public class AnalyticDto
{
    /// <summary>
    /// Gets the timestamp when the event occurred in UTC.
    /// </summary>
    /// <remarks>
    /// This property is automatically set to <see cref="DateTime.UtcNow"/> when the DTO is instantiated.
    /// </remarks>
    public DateTime Timestamp { get; private set; }

    /// <summary>
    /// Gets the type or category of the event.
    /// </summary>
    /// <remarks>
    /// This property is set during object construction and cannot be modified after creation.
    /// Common values might include "ApplicationStart", "FeatureUsed", "Error", etc.
    /// </remarks>
    public string EventType { get; private set; }

    /// <summary>
    /// Gets or sets additional event metadata stored as key-value pairs.
    /// </summary>
    /// <remarks>
    /// This dictionary allows clients to attach arbitrary contextual information to the event.
    /// Values are stored as strings; clients should serialize complex objects to JSON strings if needed.
    /// </remarks>
    public Dictionary<string, string> Metadata { get; set; } = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="AnalyticDto"/> class.
    /// </summary>
    /// <param name="eventType">The type or category of the event.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="eventType"/> is null.</exception>
    public AnalyticDto(string eventType)
    {
        EventType = eventType;
        Timestamp = DateTime.UtcNow;
    }
}
