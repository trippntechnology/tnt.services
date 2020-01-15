using System;

namespace TNT.Updater
{
	public class Settings
	{
		public string ApiEndpoint { get; set; }
		public string TokenEndpoint { get; set; }

		public Uri ApiEndpointUri { get { return new Uri(this.ApiEndpoint); } }

		public Uri TokenEndpointUri { get { return new Uri(this.TokenEndpoint); } }
	}
}
