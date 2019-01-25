using System;
using TNT.Utilities.Console;

namespace TNT.Upload.Utility
{
	class Parameters : TNT.Utilities.Console.Parameters
	{
		const string APP_ID = "i";
		const string VERSION = "v";
		const string FILE = "f";

		public int ApplicationId { get { return (this[APP_ID] as IntParameter).Value; } }

		public Version Version { get { return (this[VERSION] as VersionParameter).Value; } }

		public string FilePath { get { return (this[FILE] as FileParameter).Value; } }

		public Parameters()
		{
			this.Add(new IntParameter(APP_ID, "ID of the application to update", true));
			this.Add(new VersionParameter(VERSION, "Version of the application"));
			this.Add(new FileParameter(FILE, "Name of file to upload", true));
		}

	}
}
