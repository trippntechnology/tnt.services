using TNT.Services.Service.Models.Entities;

namespace TNT.Services.Service.Models
{
  public class ApplicationPlus : Application
  {
    public string Version { get; set; } = String.Empty;
    public string FileName { get; set; } = String.Empty;
    public DateTime? Date { get; set; } = null;
  }
}
