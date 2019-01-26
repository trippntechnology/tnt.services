using Newtonsoft.Json;
using RestSharp;
using System;
using System.Diagnostics;
using System.IO;
using TNT.Services.Models;

namespace TNT.Services.Client
{
	public class Client
	{
		const string UPLOAD_ENDPOINT = "/Upload/";

		protected IRestClient _RestClient;

		public Client(Uri apiUri)
		{
			_RestClient = new RestClient(apiUri);
		}

		public Response Upload(int applicationId, string fileName)
		{
			Response response = null;

			try
			{
				var fileBytes = File.ReadAllBytes(fileName);

				var releaseRequest = new ReleaseRequest()
				{
					ApplicationID = applicationId,
					Version = GetVersion(fileName),
					Base64EncodedFile = Convert.ToBase64String(fileBytes)
				};

				var restRequest = new RestRequest(UPLOAD_ENDPOINT, Method.POST, DataFormat.Json).AddJsonBody(releaseRequest);
				var restResponse = _RestClient.Execute(restRequest);

				if (restResponse.IsSuccessful)
				{
					response = JsonConvert.DeserializeObject<Response>(restResponse.Content);
				}
				else
				{
					response = new Response(restResponse.ErrorException);
				}
			}
			catch (Exception error)
			{
				response = new Response(error);
			}

			return response;
		}

		private static string GetVersion(string fileName)
		{
			var fvi = FileVersionInfo.GetVersionInfo(fileName);
			return fvi.FileVersion;
		}
	}
}
