using System;

namespace TNT.Services.Objects
{
	/// <summary>
	/// Represents a user associated with a license
	/// </summary>
	public class User
	{
		/// <summary>
		/// Name
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Email address
		/// </summary>
		public string EmailAddress { get; set; }

		/// <summary>
		/// Street address
		/// </summary>
		public string Address { get; set; }

		/// <summary>
		/// City
		/// </summary>
		public string City { get; set; }

		/// <summary>
		/// State
		/// </summary>
		public string State { get; set; }

		/// <summary>
		/// Zip code
		/// </summary>
		public string Zip { get; set; }

		/// <summary>
		/// Phone number
		/// </summary>
		public string PhoneNumber { get; set; }
	}
}
