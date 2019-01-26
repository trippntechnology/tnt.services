using System;

namespace TNT.Services.Models.Exceptions
{
	/// <summary>
	/// <see cref="Exception"/> indicating that the ID specified for the application is not valid.
	/// </summary>
	public class InvalidApplicationIdException : Exception
	{
		private const string DEFAULT_MESSAGE = "Invalid Application ID";

		/// <summary>
		/// Default constructor
		/// </summary>
		/// <param name="message">Message that can be included with the exception</param>
		public InvalidApplicationIdException(string message = DEFAULT_MESSAGE) : base(message)
		{

		}
	}
}
