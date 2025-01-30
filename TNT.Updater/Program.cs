using TNT.Services.Client;
using TNT.Services.Models;
using TNT.Services.Models.Dto;
using TNT.Updater.Properties;

namespace TNT.Updater;

static class Program
{
  /// <summary>
  /// The main entry point for the application.
  /// </summary>
  [STAThread]
  static void Main(string[] args)
  {
    Application.EnableVisualStyles();
    Application.SetCompatibleTextRenderingDefault(false);

    var arguments = new Arguments();

    try
    {
      //args = new List<string>
      //{
      //  "",
      //  "/a", "TNT.Updater.exe",
      //  "/i", "fa1f92e1-4beb-4675-9290-0af2265909a2",
      //  "/p", "2gHpq!TYt9xop65hSp",
      //  "/e", "https://localhost:5001/api"
      //}.ToArray();

      arguments.Parse(args, false);
    }
    catch (Exception ex)
    {
      MessageBox.Show(arguments.Usage(ex), "Usage", MessageBoxButtons.OK, MessageBoxIcon.Information);
      return;
    }

    ApplicationInfoDto? appInfo = null;
    AuthenticatedClient? authClient = null;

    try
    {
      Client client = new Client(arguments.BaseUri);
      DtoResponse<Services.Models.JWT> jwtResponse = client.Authorize(arguments.ApplicationId, arguments.ApplicationPassword);

      if (!jwtResponse.IsSuccess) throw new Exception(jwtResponse.Message);
      if (jwtResponse.Data == null) throw new Exception("Token is null");

      authClient = new AuthenticatedClient(arguments.BaseUri, jwtResponse.Data);
      DtoResponse<ApplicationInfoDto> appInfoResponse = authClient.ApplicationInfo(arguments.ApplicationId);
      if (!appInfoResponse.IsSuccess) throw new Exception(appInfoResponse.Message);
      appInfo = appInfoResponse.Data;
    }
    catch (Exception ex)
    {
      if (!arguments.IsSilentMode)
      {
        var caption = string.Format(Resources.Caption, arguments.ProductName);
        MessageBox.Show(ex.Message, caption, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
      }
      return;
    }

    //Console.WriteLine(appInfo.ToString());
    Version installedVersion = arguments.FileVersion;
    Version currentVersion = Version.Parse(appInfo!.ReleaseVersion ?? "0.0.0");

    if (authClient == null || (arguments.IsSilentMode && installedVersion.CompareTo(currentVersion) >= 0))
    {
      return;
    }

    Application.Run(new Form1(arguments, appInfo, authClient));
  }
}
