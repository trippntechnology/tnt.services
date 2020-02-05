using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using TNT.ArgumentParser;

namespace TNT.Updater
{
	public class Arguments : ArgumentParser.ArgumentParser
	{
		private const string API_CONTROLLER = "v1";
		private const string AUTH_CONTROLLER = "Authorization";

		private const string APPLICATION = "a";
		private const string APP_ID = "i";
		private const string APP_PASSWORD = "p";
		private const string SILENT_MODE = "s";
		private const string ENDPOINT = "e";
		private const string WRITE_TO_CONSOLE = "c";

		public bool WriteToConsole { get { return (this[WRITE_TO_CONSOLE] as FlagArgument).Value; } }
		public string Executable { get { return (this[APPLICATION] as FileArgument).Value; } }
		public int ApplicationId { get { return (this[APP_ID] as IntArgument).Value; } }
		public string ApplicationPassword { get { return (this[APP_PASSWORD] as StringArgument).Value; } }
		public bool IsSilentMode { get { return (this[SILENT_MODE] as FlagArgument).Value; } }
		public Uri BaseUri { get { return (this[ENDPOINT] as UriArgument).Value; } }
		public Uri ApiEndoint { get { return new Uri($"{BaseUri.ToString()}/{API_CONTROLLER}"); } }
		public Uri AuthEnpoint { get { return new Uri($"{BaseUri.ToString()}/{AUTH_CONTROLLER}"); } }

		public string CompanyName { get; private set; }
		public string ProductName { get; private set; }
		public Version FileVersion { get; private set; }
		public string ProcessName { get { return Path.GetFileNameWithoutExtension(Executable); } }

		public Arguments() : base()
		{
			Add(new FileArgument(APPLICATION, "Application to update", true, true));
			Add(new IntArgument(APP_ID, "Application ID", true));
			Add(new StringArgument(APP_PASSWORD, "Application password", true));
			Add(new FlagArgument(SILENT_MODE, "Hides dialog when latest version is installed"));
			Add(new UriArgument(ENDPOINT, "Base Uri to the service", true));
			Add(new FlagArgument(WRITE_TO_CONSOLE, "Forces usage to be written to console"));
		}

		public string Usage(Exception ex)
		{
			var sb = new StringBuilder(ex.Message);
			sb.AppendLine();
			sb.AppendLine();
			sb.AppendLine(base.Usage());
			return sb.ToString();
		}

		public override bool Parse(string[] args, bool swallowException = true)
		{
			try
			{
				base.Parse(args, false);

				FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(this.Executable);
				this.CompanyName = fvi.CompanyName;
				this.ProductName = fvi.ProductName;
				this.FileVersion = Version.Parse(fvi.FileVersion);
			}
			catch (Exception ex)
			{
				throw ex;
			}

			return true;
		}
	}
}
