using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace TNT.Update.Service.Models.Entities
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
