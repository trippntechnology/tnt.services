using System;
using TNT.Services.Objects;
using TNT.WCF.Utilities;

namespace TNT.Services.Contracts
{
	/// <summary>
	/// Represents a TNT services client
	/// </summary>
	public class TNTServicesClient : ServiceClient<ITNTServicesContracts>
	{

		#region Constructors

		/// <summary>
		/// Initializes using and endpoint
		/// </summary>
		/// <param name="endpointConfigurationName">Endpoint specified in the configuration</param>
		public TNTServicesClient(string endpointConfigurationName)
			: base(endpointConfigurationName)
		{
		}

		#endregion

		/// <summary>
		/// Tests the connectivity from the client to the service
		/// </summary>
		/// <returns>True</returns>
		public bool TestConnectivity()
		{
			return Execute<bool>(channel =>
			{
				return channel.TestConnectivity();
			});
		}

		/// <summary>
		/// Gets an application license for a user
		/// </summary>
		/// <param name="user">User</param>
		/// <param name="applicationID">Application's ID</param>
		/// <returns>Application license for a user</returns>
		public License RequestLicense(User user, Guid applicationID)
		{
			return Execute<License>(channel =>
			{
				return channel.RequestLicense(user, applicationID);
			});
		}

		/// <summary>
		/// Get the application info for an application ID
		/// </summary>
		/// <param name="applicationID">ID associated with the application in question</param>
		/// <returns>Application info for an application ID if exist, null otherwise</returns>
		public Application GetApplicationInfo(Guid applicationID)
		{
			return Execute<Application>(channel =>
			{
				return channel.GetApplicationInfo(applicationID);
			});
		}

		/// <summary>
		/// Gets the authorization key associated with the hardware ID given a valid license key for an
		/// application ID
		/// </summary>
		/// <param name="hardwareID">Identifier belonging to one of the hardware components</param>
		/// <param name="applicationID">Identifier associated with the application</param>
		/// <param name="licenseKey">License key issued during registration</param>
		/// <returns>
		/// String that represents and authorization key if license key and application ID are valid, 
		/// string.empty otherwise
		/// </returns>
		public string GetAuthorizationKey(string hardwareID, Guid applicationID, string licenseKey)
		{
			return Execute<string>(channel =>
			{
				return channel.GetAuthorizationKey(hardwareID, applicationID, licenseKey);
			});
		}
	}
}