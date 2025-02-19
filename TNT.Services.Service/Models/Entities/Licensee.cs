﻿using System.ComponentModel.DataAnnotations;

namespace TNT.Services.Service.Models.Entities;

public class Licensee
{
  public Guid ID { get; set; } = Guid.NewGuid();
  public string Name { get; set; } = string.Empty;
  public Guid ApplicationId { get; set; }

  [Display(Name = "Valid Until")]
  public DateTimeOffset ValidUntil { get; set; }

  public string FormattedExpirationDate { get { return ValidUntil.ToString("MM/dd/yyyy"); } }

  public Licensee() { }

  public Licensee(Licensee licensee)
  {
    ID = licensee.ID;
    Name = licensee.Name;
    ApplicationId = licensee.ApplicationId;
    ValidUntil = licensee.ValidUntil;
  }
}
