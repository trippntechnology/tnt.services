﻿using System;

namespace TNT.Update.Models
{
	/// <summary>
	/// Base response to API requests
	/// </summary>
	public class Response
	{
		/// <summary>
		/// Indicates the result of responding to a request
		/// </summary>
		public bool IsSuccess { get; set; } = true;

		/// <summary>
		/// Message that can provide my details about the response
		/// </summary>
		public string Message { get; set; } = string.Empty;

		public Response()
		{

		}

		/// <summary>
		/// Default constructor
		/// </summary>
		/// <param name="isSuccess">Set to indicate whether the request was a success or failure. Default is true.</param>
		/// <param name="message">Optional message that can be supplied to provide more information about the response.</param>
		//public Response(bool isSuccess = true, string message = "")
		//{

		//}

		/// <summary>
		/// Initializes a new <see cref="Response"/> with a message generated by <paramref name="ex"/> and sets <see cref="IsSuccess"/> to false
		/// </summary>
		/// <param name="ex">Exception used to generate the failed response.</param>
		public Response(Exception ex)
		{
			this.IsSuccess = false;
			this.Message = $"{ex.GetType().Name}: {ex.Message}";
		}
	}
}
