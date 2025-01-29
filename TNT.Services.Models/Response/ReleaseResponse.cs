using System;

namespace TNT.Services.Models.Response
{
  public class ReleaseResponse : Response
  {
    public DateTimeOffset? ReleaseDate { get; set; } = null;
    public string Package { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;

    public ReleaseResponse() : base() { }

    public ReleaseResponse(Exception ex) : base(ex) { }
  }
}
