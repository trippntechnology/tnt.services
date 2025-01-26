namespace TNT.Services.Models.Dto;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
public class ApplicationInfoDto
{
  public Guid ApplicationId { get; set; }

  public string? Name { get; set; } = null;

  public string? ReleaseVersion { get; set; } = null;

  public DateTimeOffset? ReleaseDate { get; set; } = null;

  public int ReleaseID { get; set; } = -1;

  public ApplicationInfoDto() { }
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
