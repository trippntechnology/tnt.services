using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using TNT.Services.Client;
using TNT.Services.Models.Response;
using TNT.Updater.Properties;

namespace TNT.Updater
{
	public partial class Form1 : Form
	{
		private readonly Arguments arguments;
		private readonly Client client;
		private readonly ApplicationInfo appInfo;

		private ReleaseResponse releaseResponse = null;

		public Form1(Arguments arguments, ApplicationInfo appInfo, Settings settings)
		{
			InitializeComponent();
			this.arguments = arguments;
			this.appInfo = appInfo;
			this.client = new Client(settings.ApiEndpointUri, settings.TokenEndpointUri);

			Log("Calling InitializeAsync");
			InitializeAsync();
			Log("Exiting Form1()");
		}

		public void InitializeAsync()
		{
			Text = string.Format(Resources.Caption, this.arguments.ProductName);
			labelInstalledVersion.Text = this.arguments.FileVersion.ToString();
			labelCurrentVersion.Text = appInfo.ReleaseVersion;
			labelReleaseDate.Text = appInfo.ReleaseDate.ToString();

			var installedVersion = this.arguments.FileVersion;
			var currentVersion = Version.Parse(appInfo.ReleaseVersion);

			buttonInstall.Enabled = currentVersion > installedVersion;

			if (arguments.IsSilentMode && installedVersion == currentVersion)
			{
				Close();
			}
		}

		private async void buttonInstall_Click(object sender, EventArgs e)
		{
			if (releaseResponse == null)
			{
				var jwt = client.GetJWT(arguments.ApplicationId, arguments.ApplicationPassword);
				var response = await client.GetReleaseAsync(appInfo.ReleaseID, jwt);
				if (response == null || !response.IsSuccess)
				{
					MessageBox.Show(response.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
				releaseResponse = response;
			}

			if (ProcessIsRunning(arguments.ProcessName))
			{
				MessageBox.Show(string.Format(Resources.CloseApplicationMessage, arguments.ProcessName), Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			else
			{
				var path = Path.Combine(Path.GetTempPath(), releaseResponse.FileName);
				var package = Convert.FromBase64String(releaseResponse.Package);
				File.WriteAllBytes(path, package);

				var process = new Process();
				process.StartInfo.FileName = path;
				if (process.Start())
				{
					Close();
				}
			}
		}

		private bool ProcessIsRunning(string processName) => Process.GetProcessesByName(processName).Length > 0;

		protected void Log(string format, params object[] args)
		{
			var message = string.Format(format, args);
			Debug.WriteLine($"{DateTime.Now.ToLongTimeString()}: {message}");
		}
	}
}
