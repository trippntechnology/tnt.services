using System;

namespace TNT.Services.Models.Response
{
	public class JWTResponse : Response
	{
		public JWT Token { get; private set; }

		public JWTResponse(string token) : base() { this.Token = new JWT(token); }

		public JWTResponse(Exception ex) : base(ex) { }
	}
}
