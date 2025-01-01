namespace TNT.Services.Models.Request;

public class LicenseeRequest
{
  public Guid LicenseeId { get; set; }
  public Guid ApplicationId { get; set; }

  public override string ToString()
  {
    return $"{LicenseeId}, {ApplicationId}";
  }
}
