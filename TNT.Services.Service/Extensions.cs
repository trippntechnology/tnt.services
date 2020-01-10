using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TNT.Services.Service
{
	public static class Extensions
	{
		public static Version ToVersion(this string value)
		{
			if (Version.TryParse(value, out Version version))
			{
				return version;
			}
			else
			{
				return new Version();
			}
		}
	}
}
