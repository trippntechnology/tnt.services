namespace TNT.Services.Models
{
	public class ReleaseRequest
	{
		public string FileName { get; set; }
		public int ApplicationID { get; set; }
		public string Base64EncodedFile { get; set; }
	}
}
