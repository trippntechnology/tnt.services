﻿using TNT.Services.Client;
using TNT.Services.Models.Response;
using TNT.Updater.Properties;

namespace TNT.Updater;

static class Program
{
  /// <summary>
  /// The main entry point for the application.
  /// </summary>
  [STAThread]
  static void Main()
  {
    Application.EnableVisualStyles();
    Application.SetCompatibleTextRenderingDefault(false);

    var arguments = new Arguments();

    try
    {
      string[] args = Environment.GetCommandLineArgs();
      arguments.Parse(args.Skip(1).ToArray(), false);
    }
    catch (Exception ex)
    {
      if (!arguments.WriteToConsole)
      {
        MessageBox.Show(arguments.Usage(ex), "Usage", MessageBoxButtons.OK, MessageBoxIcon.Information);
      }
      return;
    }

    ApplicationInfo? appInfo = null;
    try
    {
      Client client = new Client(arguments.BaseUri);
      JWTResponse jwtResponse = client.GetJWT(arguments.ApplicationId, arguments.ApplicationPassword);

      if (!jwtResponse.IsSuccess) throw new Exception(jwtResponse.Message);
      if (jwtResponse.Token == null) throw new Exception("Token is null");
      appInfo = client.GetApplicationInfo(arguments.ApplicationId, jwtResponse.Token);
      if (!appInfo.IsSuccess) throw new Exception(appInfo.Message);
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

    if (arguments.IsSilentMode && installedVersion.CompareTo(currentVersion) >= 0)
    {
      return;
    }

    Application.Run(new Form1(arguments, appInfo));
  }
}
