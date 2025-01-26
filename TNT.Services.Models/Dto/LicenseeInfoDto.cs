namespace TNT.Services.Models.Dto;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
public class LicenseeInfoDto
{
  public Guid ID { get; set; } = Guid.Empty;
  public string Name { get; set; } = string.Empty;
  public Guid ApplicationId { get; set; }
  public DateTimeOffset ValidUntil { get; set; }

  public LicenseeInfoDto() { }
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
