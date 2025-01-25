using System.ComponentModel.DataAnnotations;
using TNT.Services.Service.Models.Entities;

namespace TNT.Services.Service.Models;

public class ReleasePlus : Release
{
  [Display(Name = "Application Name")]
  public string ApplicationName { get; [Display(Name = "Application Name")] set; } = String.Empty;
}