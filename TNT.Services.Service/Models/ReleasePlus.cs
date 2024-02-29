using TNT.Services.Service.Models.Entities;

namespace TNT.Services.Service.Models
{
  public class ReleasePlus : Release
  {
    public string ApplicationName { get; set; } = String.Empty;
  }
}