using System.ComponentModel.DataAnnotations;

namespace TNT.Services.Service.Models.Entities;

/// <summary>
/// Represents an analytics event recorded in the system.
/// </summary>
public class Analytic
{
    /// <summary>
    /// Gets or sets the unique identifier for the analytic record.
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the event occurred in UTC.
    /// </summary>
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Gets or sets the type or category of the event.
    /// </summary>
    public string EventType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets additional event metadata stored as JSON.
    /// </summary>
    public string Metadata { get; set; } = string.Empty;
}
