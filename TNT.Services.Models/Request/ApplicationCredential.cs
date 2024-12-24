namespace TNT.Services.Models.Request
{
  public class ApplicationCredential
  {
    public int ID { get; set; }
    public string Secret { get; set; } = string.Empty;

    public override string ToString()
    {
      return $"ID:{ID}, Secret:{Secret}";
    }
  }
}
