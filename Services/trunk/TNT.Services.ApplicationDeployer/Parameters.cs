using System;
using System.Configuration;
using System.IO;
using TNT.Utilities.Console;

namespace TNT.Services.ApplicationDeployer
{
	public class Parameters : TNT.Utilities.Console.Parameters
	{
		static string APP_PATH = "ap";
		static string APP_ID = "id";
		static string HOST = "h";
		static string FTP_USER = "u";
		static string FTP_PASSWORD = "p";
		static string SILENT = "s";

		public string AppPath { get { return Path.GetFullPath((this[APP_PATH] as FileParameter).Value); } }
		public Guid AppID { get { return new Guid((this[APP_ID] as StringParameter).Value); } }
		public string Host { get { return (this[HOST] as StringParameter).Value; } }
		public string FTPUser { get { return (this[FTP_USER] as StringParameter).Value; } }
		public string FTPPassword { get { return (this[FTP_PASSWORD] as StringParameter).Value; } }
		public string FTPPath { get { return string.Concat("ftp://", this.Host, Path.GetFileName(this.AppPath)); } }
		public string URLPath { get { return string.Concat("http://", this.Host, Path.GetFileName(this.AppPath)); } }
		public bool RunUnattended { get { return this.FlagExists(SILENT); } }

		public Parameters()
		{
			this.Add(new FileParameter(APP_PATH, "Path to LSD executable", true));
			this.Add(new StringParameter(APP_ID, "Application ID", true));
			this.Add(new StringParameter(HOST, "FTP host address", ConfigurationManager.AppSettings["Host"]));
			this.Add(new StringParameter(FTP_USER, "FTP user name", ConfigurationManager.AppSettings["FTPUser"]));
			this.Add(new StringParameter(FTP_PASSWORD, "FTP password", true));
			this.Add(new FlagParameter(SILENT, "Run unattended"));
		}
	}
}
