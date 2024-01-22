namespace TNT.Services.Models.Request
{
	public class UpdateRequest : ApplicationRequest
	{
		public string? CurrentVersion { get; set; } = null;
	}
}
