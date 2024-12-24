﻿namespace TNT.Services.Models.Request;

public class LicenseeRequest
{
  public Guid LicenseeId { get; set; }
  public int ApplicationId { get; set; }

  public override string ToString()
  {
    return $"{LicenseeId}, {ApplicationId}";
  }
}
