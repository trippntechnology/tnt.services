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
			var parameters = new Parameters();
			var client = new Client(new Uri("https://localhost:44328/api/v1"));

			if (!parameters.ParseArgs(args)) return;

			var fi = new FileInfo(parameters.FilePath);
			var fvi = FileVersionInfo.GetVersionInfo(fi.FullName);

			var response = client.Upload(parameters.ApplicationId, fvi.FileName);

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
