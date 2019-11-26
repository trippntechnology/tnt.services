using System;
using TNT.ArgumentParser;

namespace TNT.Upload.Utility
{
	class Arguments : ArgumentParser.ArgumentParser
	{
		const string APP_ID = "i";
		const string VERSION = "v";
		const string FILE = "f";

		public int ApplicationId { get { return (this[APP_ID] as IntArgument ).Value; } }

		public Version Version { get { return (this[VERSION] as VersionArgument).Value; } }

		public string FilePath { get { return (this[FILE] as FileArgument).Value; } }

		public Arguments()
		{
			this.Add(new IntArgument(APP_ID, "ID of the application to update", true));
			this.Add(new VersionArgument(VERSION, "Version of the application"));
			this.Add(new FileArgument(FILE, "Name of file to upload", true));
		}
	}
}
