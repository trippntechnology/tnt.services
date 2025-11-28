using System.Diagnostics;
using TNT.Commons;
using TNT.Services.Client;
using TNT.Services.Models;
using TNT.Services.Models.Dto;
using TNT.Updater.Properties;

namespace TNT.Updater
{
    public partial class Form1 : Form
    {
        private readonly Arguments arguments;
        private readonly AuthenticatedClient client;
        private readonly ApplicationInfoDto appInfo;

        public Form1(Arguments arguments, ApplicationInfoDto appInfo, AuthenticatedClient authClient)
        {
            InitializeComponent();
            this.arguments = arguments;
            this.appInfo = appInfo;
            this.client = authClient;

            Log("Calling InitializeAsync");
            InitializeAsync();
            Log("Exiting Form1()");
        }

        public void InitializeAsync()
        {
            Text = string.Format(Resources.Caption, this.arguments.ProductName);
            labelInstalledVersion.Text = this.arguments.FileVersion?.ToString();
            labelCurrentVersion.Text = appInfo.ReleaseVersion;
            labelReleaseDate.Text = appInfo.ReleaseDate?.DateTime.ToString();

            var installedVersion = this.arguments.FileVersion;
            var currentVersion = appInfo.ReleaseVersion?.Let(it => Version.Parse(it));

            buttonInstall.Enabled = currentVersion > installedVersion;

            if (arguments.IsSilentMode && installedVersion == currentVersion)
            {
                Close();
            }
        }

        private void buttonInstall_Click(object sender, EventArgs e)
        {
            DtoResponse<ReleaseInfoDto> response = client.ReleaseInfo(appInfo.ReleaseID);
            if (!response.IsSuccess)
            {
                MessageBox.Show(response?.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (response.Data == null) return;

            if (ProcessIsRunning(arguments.ProcessName))
            {
                MessageBox.Show(string.Format(Resources.CloseApplicationMessage, arguments.ProcessName), Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                var path = Path.Combine(Path.GetTempPath(), response.Data.FileName ?? string.Empty);
                var package = Convert.FromBase64String(response.Data.Package);
                File.WriteAllBytes(path, package);

                var process = new Process();
                process.StartInfo.FileName = path;
                if (process.Start())
                {
                    Close();
                }
            }
        }

        private bool ProcessIsRunning(string? processName) => Process.GetProcessesByName(processName).Length > 0;

        protected void Log(string format, params object[] args)
        {
            var message = string.Format(format, args);
            Debug.WriteLine($"{DateTime.Now.ToLongTimeString()}: {message}");
        }
    }
}
