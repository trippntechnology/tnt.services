namespace TNT.Services.Service.Models.Entities
{
	public class Release
	{
		public int ID { get; set; }
		public int ApplicationID { get; set; }
		public string FileName { get; set; }
		public string Version { get; set; }
		public byte[] Package { get; set; }
	}
}
