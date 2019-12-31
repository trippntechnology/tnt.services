using System;

namespace TNT.Services.Models.Response
{
	public class ApplicationInfo : Response
	{
		public string Name { get; set; }
		public string ReleaseVersion { get; set; }
		public DateTime ReleaseDate { get; set; }
		public int ReleaseID { get; set; }

		public ApplicationInfo() : base() { }

		public ApplicationInfo(Exception ex) : base(ex) { }
	}
}
