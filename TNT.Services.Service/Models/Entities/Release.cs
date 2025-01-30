using System.ComponentModel.DataAnnotations;

namespace TNT.Services.Service.Models.Entities;

public class Release
{
  public int ID { get; set; }
  public Guid ApplicationID { get; set; }

  [Display(Name = "File Name")]
  public string FileName { get; set; } = String.Empty;
  public string Version { get; set; } = String.Empty;
  public byte[] Package { get; set; } = new byte[0];
  public DateTime Date { get; set; }
}
