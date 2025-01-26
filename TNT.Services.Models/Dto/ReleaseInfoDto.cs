namespace TNT.Services.Models.Dto
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
  public class ReleaseInfoDto
  {
    public int Id { get; set; }
    public DateTimeOffset? ReleaseDate { get; set; } = null;
    public string Package { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;

    public ReleaseInfoDto() { }
  }
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
