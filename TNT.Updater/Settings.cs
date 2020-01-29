using System;

namespace TNT.Updater
{
	public class Settings
	{
		private const string API_CONTROLLER = "v1";
		private const string TOKEN_CONTROLLER = "token";

		public string ApiEndpoint { get; set; }

		public Uri ApiEndpointUri { get { return new Uri($"{this.ApiEndpoint}{API_CONTROLLER}"); } }

		public Uri TokenEndpointUri { get { return new Uri($"{this.ApiEndpoint}{TOKEN_CONTROLLER}"); } }
	}
}
