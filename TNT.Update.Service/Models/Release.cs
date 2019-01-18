using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TNT.Update.Service.Models
{
	public class Release
	{
		public int ID { get; set; }
		public int ApplicationID { get; set; }
		public byte[] Package { get; set; }
	}
}
