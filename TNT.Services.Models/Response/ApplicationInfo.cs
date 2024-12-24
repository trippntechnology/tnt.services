using System.Text;

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

    public override string ToString()
    {
      var sb = new StringBuilder();
      sb.AppendLine($"Name: {Name}");
      sb.AppendLine($"ReleaseVersion: {ReleaseVersion}");
      sb.AppendLine($"ReleaseDate: {ReleaseDate}");
      sb.AppendLine($"ReleaseID: {ReleaseID}");
      return sb.ToString();
    }
  }
}
