using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using TNT.ArgumentParser;

namespace TNT.Updater
{
	public class Arguments : ArgumentParser.ArgumentParser
	{
		private const string EXECUTABLE = "e";
		private const string APP_ID = "i";
		private const string APP_PASSWORD = "p";
		private const string SILENT_MODE = "s";

		public string Executable { get { return (this[EXECUTABLE] as FileArgument).Value; } }
		public int ApplicationId { get { return (this[APP_ID] as IntArgument).Value; } }
		public string ApplicationPassword { get { return (this[APP_PASSWORD] as StringArgument).Value; } }
		public bool IsSilentMode { get { return (this[SILENT_MODE] as FlagArgument).Value; } }

		public string CompanyName { get; private set; }
		public string ProductName { get; private set; }
		public Version FileVersion { get; private set; }
		public string ProcessName { get { return Path.GetFileNameWithoutExtension(Executable); } }

		public Arguments() : base()
		{
			Add(new FileArgument(EXECUTABLE, "Application to update", true, true));
			Add(new IntArgument(APP_ID, "Application ID", true));
			Add(new StringArgument(APP_PASSWORD, "Application password", true));
			Add(new FlagArgument(SILENT_MODE, "Hides dialog when latest version is installed"));
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
