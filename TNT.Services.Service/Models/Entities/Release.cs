namespace TNT.Services.Service.Models.Entities
{
  public class Release
  {
    public int ID { get; set; }
    public int ApplicationID { get; set; }
    public string FileName { get; set; } = String.Empty;
    public string Version { get; set; } = String.Empty;
    public byte[]? Package { get; set; } = null;
    public DateTime? Date { get; set; } = null;
  }
}
