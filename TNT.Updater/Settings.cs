using System;

namespace TNT.Updater
{
	public class Settings
	{
		public string Endpoint { get; set; }

		public Uri EndpointUri { get { return new Uri(this.Endpoint); } }
	}
}
