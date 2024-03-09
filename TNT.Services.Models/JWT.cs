namespace TNT.Services.Models;

public class JWT
{
  public string Raw { get; private set; }
  public string Header { get { return Raw.Split('.')[0]; } }
  public string Payload { get { return Raw.Split('.')[1]; } }
  public string Signature { get { return Raw.Split('.')[2]; } }
  public string ToAuthToken { get { return $"Bearer {this.Raw}"; } }

  public JWT(string jwt)
  {
    this.Raw = jwt;
  }

  public override string ToString() => this.Raw;
}
