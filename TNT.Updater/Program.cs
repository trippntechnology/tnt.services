using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TNT.Updater
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			var arguments = new Arguments();

			try
			{
				var args = Environment.GetCommandLineArgs();
				arguments.Parse(args.Skip(1).ToArray(), false);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
				return;
			}
			System.Diagnostics.Debugger.Launch();

			Application.Run(new Form1());
		}
	}
}
