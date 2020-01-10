using System;
using System.Collections.Generic;
using System.Text;

namespace TNT.Services.Models.Exceptions
{
	public class ReleaseNotFoundException : Exception
	{
		private const string DEFAULT_MESSAGE = "Release associated with application ID, {0}, could not be found";

		public ReleaseNotFoundException(int id, string message = DEFAULT_MESSAGE) : base(string.Format(message, id)) { }
	}
}
