namespace TNT.Services.Models.Response;

public class LicenseeResponse : Response
{
  public string ID { get; set; } = string.Empty;
  public string Name { get; set; } = string.Empty;
  public int ApplicationId { get; set; }
  public DateTimeOffset ValidUntil { get; set; }

  public LicenseeResponse() : base() { }
  public LicenseeResponse(Exception ex) : base(ex) { }
}
