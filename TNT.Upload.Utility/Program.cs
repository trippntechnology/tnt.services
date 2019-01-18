using Newtonsoft.Json;
using RestSharp;
using System;
using System.IO;
using TNT.Update.Models;

namespace TNT.Upload.Utility
{
	class Program
	{
		static void Main(string[] args)
		{
			//System.Diagnostics.Debugger.Launch();
			var relReq = new ReleaseRequest();

			relReq.ApplicationID = int.Parse(args[0]);
			relReq.Version = args[1];
			string fileName = args[2];

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
				var response = client.Execute(request);
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
