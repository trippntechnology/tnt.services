using Newtonsoft.Json;
using RestSharp;
using System;
using System.Threading.Tasks;
using TNT.Services.Models;
using TNT.Services.Models.Request;
using TNT.Services.Models.Response;

namespace TNT.Services.Client
{
	public class Client
	{
		private const string APPLICATION_INFO_ENDPOINT = "/GetApplicationInfo/";
		private const string RELEASE_ENDPOINT = "/GetRelease/";
		private const string AUTHORIZE = "/Authorize/";

		protected IRestClient apiClient;
		protected IRestClient tokenClient;

		public Client(Uri apiUri, Uri tokenUri)
		{
			apiClient = new RestClient(apiUri);
			tokenClient = new RestClient(tokenUri);
		}

		public ApplicationInfo GetApplicationInfo(int appId, JWT jwt)
		{
			var appRequest = new ApplicationRequest() { ApplicationID = appId };
			var request = new RestRequest(APPLICATION_INFO_ENDPOINT, Method.POST, DataFormat.Json)
				.AddHeader("Authorization", jwt.ToAuthToken)
				.AddJsonBody(appRequest);
			var response = apiClient.Execute(request);

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

		public Task<ApplicationInfo> GetApplicationInfoAsync(int appid, JWT jwt)
		{
			return Task.Run(() => { return GetApplicationInfo(appid, jwt); });
		}

		public ReleaseResponse GetRelease(int releaseId, JWT jwt)
		{
			var releaseRequest = new ReleaseRequest() { ReleaseId = releaseId };
			var request = new RestRequest(RELEASE_ENDPOINT, Method.POST, DataFormat.Json)
				.AddHeader("Authorization", jwt.ToAuthToken)
				.AddJsonBody(releaseRequest);
			var response = apiClient.Execute(request);

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

		public Task<ReleaseResponse> GetReleaseAsync(int releaseId, JWT jwt)
		{
			return Task.Run(() => { return GetRelease(releaseId, jwt); });
		}

		public JWTResponse GetJWT(int appId, string password)
		{
			var appCredential = new ApplicationCredential() { ID = appId, Secret = password };
			var request = new RestRequest(AUTHORIZE, Method.POST).AddJsonBody(appCredential);
			var response = tokenClient.Execute(request);
			JWTResponse jwtResponse;
			var content = JsonConvert.DeserializeObject<string>(response.Content);
			if (response.IsSuccessful)
			{
				jwtResponse = new JWTResponse(content);
			}
			else
			{
				jwtResponse = new JWTResponse(new Exception(content));
			}
			return jwtResponse;
		}
	}
}
