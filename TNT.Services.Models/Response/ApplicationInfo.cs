namespace TNT.Services.Models.Response
{
  public class ApplicationInfo : Response
  {
    public string? Name { get; set; } = null;
    public string? ReleaseVersion { get; set; } = null;
    public DateTime? ReleaseDate { get; set; } = null;
    public int ReleaseID { get; set; } = -1;

    public ApplicationInfo() : base() { }

    public ApplicationInfo(Exception ex) : base(ex) { }
  }
}
