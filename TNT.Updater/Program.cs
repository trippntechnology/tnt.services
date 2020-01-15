using System;
using System.Linq;
using System.Windows.Forms;
using TNT.Configuration;
using TNT.Services.Client;
using TNT.Services.Models.Response;
using TNT.Updater.Properties;

namespace TNT.Updater
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Settings settings = XmlSection<Settings>.Deserialize("SettingsSection");

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			var arguments = new Arguments();

			try
			{
				var args = Environment.GetCommandLineArgs();
				arguments.Parse(args.Skip(1).ToArray(), false);
			}
			catch (Exception ex)
			{
				MessageBox.Show(arguments.Usage(ex), "Usage", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			ApplicationInfo appInfo = null;
			try
			{
				var client = new Client(settings.ApiEndpointUri, settings.TokenEndpointUri);
				var jwt = client.GetJWT(arguments.ApplicationId, arguments.ApplicationPassword);
				appInfo = client.GetApplicationInfo(arguments.ApplicationId, jwt);
				if (!appInfo.IsSuccess) throw new Exception(appInfo.Message);
			}
			catch (Exception ex)
			{
				if (!arguments.IsSilentMode)
				{
					var caption = string.Format(Resources.Caption, arguments.ProductName);
					MessageBox.Show(ex.Message, caption, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
					return;
				}
			}

			var installedVersion = arguments.FileVersion;
			var currentVersion = Version.Parse(appInfo.ReleaseVersion);

			if (arguments.IsSilentMode && installedVersion == currentVersion)
			{
				return;
			}

			Application.Run(new Form1(arguments, appInfo, settings));
		}
	}
}
