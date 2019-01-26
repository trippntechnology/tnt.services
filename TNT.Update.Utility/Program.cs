using Newtonsoft.Json;
using RestSharp;
using System;
using System.Diagnostics;
using System.IO;
using TNT.Services.Models;

namespace TNT.Upload.Utility
{
	class Program
	{
		static void Main(string[] args)
		{
			var parameters = new Parameters();

			if (!parameters.ParseArgs(args)) return;
			System.Diagnostics.Debugger.Launch();

			var fi = new FileInfo(parameters.FilePath);

			var fvi = FileVersionInfo.GetVersionInfo(fi.FullName);
			//var ass = Assembly.LoadFile(fi.FullName);



			//var fileVersion = Utilities.Utilities.GetAssemblyAttribute<AssemblyFileVersionAttribute>(ass);
			//var infoVersion =Utilities.Utilities.GetAssemblyAttribute<AssemblyInformationalVersionAttribute>(ass);
			//var version = Utilities.Utilities.GetAssemblyAttribute<AssemblyVersionAttribute>(ass);

			//System.Diagnostics.Debugger.Launch();
			var relReq = new ReleaseRequest();

			relReq.ApplicationID = parameters.ApplicationId;
			relReq.Version = fvi.FileVersion;
			string fileName = Path.GetFileName(fvi.FileName);

			var client = new RestClient("https://localhost:44328/api/v1");

			var request = new RestRequest("/Upload/", Method.POST);

			var bytes = File.ReadAllBytes(fileName);
			relReq.Base64EncodedFile = Convert.ToBase64String(bytes);

			// Json to post.
			string jsonToSend = JsonConvert.SerializeObject(relReq);

			request.AddParameter("application/json; charset=utf-8", jsonToSend, ParameterType.RequestBody);
			request.RequestFormat = DataFormat.Json;

			try
			{
				var restResponse = client.Execute(request);

				if (restResponse.IsSuccessful)
				{
					var response = JsonConvert.DeserializeObject<Response>(restResponse.Content);

					if (response.IsSuccess)
					{
						Console.WriteLine("Application updated successfully");
					}
					else
					{
						Console.WriteLine(response.Message);
					}
				}
				//client.ExecuteAsync(request, response =>
				//{
				//	if (response.StatusCode == HttpStatusCode.OK)
				//	{
				//		// OK
				//	}
				//	else
				//	{
				//		// NOK
				//	}
				//});
			}
			catch (Exception error)
			{
				// Log
			}
		}
	}
}
