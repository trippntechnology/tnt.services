using System.ComponentModel.DataAnnotations;
using TNT.Services.Service.Models.Entities;

namespace TNT.Services.Service.Models;

public class LicenseePlus : Licensee
{
  [Display(Name = "Application Name")]
  public string ApplicationName { get; set; } = String.Empty;

  public LicenseePlus(Licensee licensee, string applictionName) : base(licensee)
  {
    ApplicationName = applictionName;
  }
}
