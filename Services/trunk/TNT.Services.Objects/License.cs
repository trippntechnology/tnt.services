using System;

namespace TNT.Services.Objects
{
	/// <summary>
	/// Represents a license record
	/// </summary>
	public class License
	{
		/// <summary>
		/// Key unique to this license
		/// </summary>
		public string Key { get; set; }

		/// <summary>
		/// Application associated with the license
		/// </summary>
		public Application Application { get; set; }

		/// <summary>
		/// User the license was issued to
		/// </summary>
		public User IssuedTo { get; set; }

		/// <summary>
		/// Date the licenses was issued
		/// </summary>
		public DateTime IssuedOn { get; set; }
	}
}
