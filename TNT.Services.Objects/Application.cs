using System;

namespace TNT.Services.Objects
{
	/// <summary>
	/// Represents an application record
	/// </summary>
	public class Application
	{
		/// <summary>
		/// Application's ID
		/// </summary>
		public Guid ID { get; set; }

		/// <summary>
		/// Application name
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Application version
		/// </summary>
		public string Version { get; set; }

		/// <summary>
		/// Application URL
		/// </summary>
		public Uri URL { get; set; }
	}
}
