using System.Text;

namespace TNT.Services.Models.Response;

public class LicenseeResponse : Response
{
  public Guid ID { get; set; } = Guid.Empty;
  public string Name { get; set; } = string.Empty;
  public Guid ApplicationId { get; set; }
  public DateTimeOffset ValidUntil { get; set; }

  public LicenseeResponse() : base() { }
  public LicenseeResponse(Exception ex) : base(ex) { }

  public override string ToString()
  {
    var sb = new StringBuilder();
    sb.AppendLine($"ID: {ID}");
    sb.AppendLine($"Name: {Name}");
    sb.AppendLine($"ApplicationId: {ApplicationId}");
    sb.AppendLine($"ValidUntil: {ValidUntil}");
    return sb.ToString();
  }
}
