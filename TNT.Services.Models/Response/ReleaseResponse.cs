using System;

namespace TNT.Services.Models.Response
{
	public class ReleaseResponse : Response
	{
		public DateTime ReleaseDate { get; set; }
		public string Package { get; set; }
		public string FileName { get; set; }

		public ReleaseResponse() : base() { }

		public ReleaseResponse(Exception ex) : base(ex) { }
	}
}
