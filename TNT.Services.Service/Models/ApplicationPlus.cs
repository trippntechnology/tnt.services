using System;
using TNT.Services.Service.Models.Entities;

namespace TNT.Services.Service.Models
{
	public class ApplicationPlus : Application
	{
		public string Version { get; set; }
		public string FileName { get; set; }
		public DateTime Date { get; set; }
	}
}
