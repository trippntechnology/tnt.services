using System;
using System.Diagnostics;
using System.IO;
using TNT.Services.Client;

namespace TNT.Upload.Utility
{
	class Program
	{
		static void Main(string[] args)
		{
			var arguments = new Arguments();
			var client = new Client(new Uri("https://localhost:5001/api/v1"));

			if (!arguments.Parse(args)) return;

			var fi = new FileInfo(arguments.FilePath);
			var fvi = FileVersionInfo.GetVersionInfo(fi.FullName);

			var response = client.Upload(arguments.ApplicationId, fvi.FileName);

			if (response.IsSuccess)
			{
				Console.WriteLine("Application updated successfully");
			}
			else
			{
				Console.WriteLine(response.Message);
			}
		}
	}
}