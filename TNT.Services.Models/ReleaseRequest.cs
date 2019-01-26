using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TNT.Services.Models
{
	public class ReleaseRequest
	{
		public int ApplicationID { get; set; }
		public string Version { get; set; }
		public string Base64EncodedFile { get; set; }
	}
}
