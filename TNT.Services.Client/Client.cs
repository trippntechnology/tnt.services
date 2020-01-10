using Newtonsoft.Json;
using RestSharp;
using System;
using System.Threading.Tasks;
using TNT.Services.Models.Request;
using TNT.Services.Models.Response;

namespace TNT.Services.Client
{
	public class Client
	{
		const string UPLOAD_ENDPOINT = "/Upload/";
		private const string APPLICATION_INFO_ENDPOINT = "/GetApplicationInfo/";
		private const string RELEASE_ENDPOINT = "/GetRelease/";

		protected IRestClient _RestClient;

		public Client(Uri apiUri)
		{
			_RestClient = new RestClient(apiUri);
		}

		public ApplicationInfo GetApplicationInfo(int appId)
		{
			var appRequest = new ApplicationRequest() { ApplicationID = appId };
			var request = new RestRequest(APPLICATION_INFO_ENDPOINT, Method.POST, DataFormat.Json).AddJsonBody(appRequest);
			var response = _RestClient.Execute(request);

			ApplicationInfo appInfo;
			if (response.IsSuccessful)
			{
				appInfo = JsonConvert.DeserializeObject<ApplicationInfo>(response.Content);
			}
			else
			{
				appInfo = new ApplicationInfo(response.ErrorException);
			}

			return appInfo;
		}

		public Task<ApplicationInfo> GetApplicationInfoAsync(int appid)
		{
			return Task.Run(() => { return GetApplicationInfo(appid); });
		}

		public ReleaseResponse GetRelease(int releaseId)
		{
			var releaseRequest = new ReleaseRequest() { ReleaseId = releaseId };
			var request = new RestRequest(RELEASE_ENDPOINT, Method.POST, DataFormat.Json).AddJsonBody(releaseRequest);
			var response = _RestClient.Execute(request);

			ReleaseResponse releaseResponse;
			if (response.IsSuccessful)
			{
				releaseResponse = JsonConvert.DeserializeObject<ReleaseResponse>(response.Content);
			}
			else
			{
				releaseResponse = new ReleaseResponse(response.ErrorException);
			}

			return releaseResponse;
		}

		public Task<ReleaseResponse> GetReleaseAsync(int releaseId)
		{
			return Task.Run(() => { return GetRelease(releaseId); });
		}
	}
}
