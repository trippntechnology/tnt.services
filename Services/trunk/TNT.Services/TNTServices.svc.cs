using System;
using TNT.Services.Contracts;
using TNT.Services.DAL;
using TNT.Services.Objects;

namespace TNT.Services
{
	/// <summary>
	/// TNT Registration service
	/// </summary>
	public class TNTServices : ITNTServicesContracts
	{
		/// <summary>
		/// Tests connectivity between the client and the service
		/// </summary>
		/// <returns>True</returns>
		public bool TestConnectivity()
		{
			return true;
		}

		public License RequestLicense(User user, Guid applicationID)
		{
			return LicenseDAL.GetNewLicense(user, applicationID);
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
			return AuthorizationDAL.GetAuthorizationKey(hardwareID, applicationID, licenseKey);
		}

		/// <summary>
		/// Get the application info for an application ID
		/// </summary>
		/// <param name="applicationID">ID associated with the application in question</param>
		/// <returns>Application info for an application ID if exist, null otherwise</returns>
		public Application GetApplicationInfo(Guid applicationID)
		{
			return ApplicationDAL.GetApplication(applicationID);
		}
	}
}
