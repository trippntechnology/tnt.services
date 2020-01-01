using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNT.ArgumentParser;

namespace TNT.Updater
{
	class Arguments : ArgumentParser.ArgumentParser
	{
		private const string APPLICATION = "a";

		public string Application { get { return (this[APPLICATION] as FileArgument).Value; } }

		public Arguments() : base()
		{
			Add(new FileArgument(APPLICATION, "Application to update", true, true));
		}
	}
}
