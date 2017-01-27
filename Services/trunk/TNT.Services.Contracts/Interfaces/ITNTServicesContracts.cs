using System;
using System.ServiceModel;
using TNT.Services.Objects;
using TNT.Services.Objects.Faults;

namespace TNT.Services.Contracts
{
	/// <summary>
	/// Represents the interface into the TNT.Registration service
	/// </summary>
	[ServiceContract(Namespace="TNT.Services.Contracts")]
	public interface ITNTServicesContracts
	{
		/// <summary>
		/// Tests the connectivity from the client to the service
		/// </summary>
		/// <returns>True</returns>
		[OperationContract]
		bool TestConnectivity();

		/// <summary>
		/// Gets an application license for a user
		/// </summary>
		/// <param name="user">User</param>
		/// <param name="applicationID">Application's ID</param>
		/// <returns>Application license for a user</returns>
		[OperationContract]
		[FaultContract(typeof(InvalidApplicationFault))]
		[FaultContract(typeof(InvalidUserFault))]
		License RequestLicense(User user, Guid applicationID);

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
		[OperationContract]
		[FaultContract(typeof(InvalidApplicationFault))]
		[FaultContract(typeof(InvalidParameterFault))]
		string GetAuthorizationKey(string hardwareID, Guid applicationID, string licenseKey);

		/// <summary>
		/// Get the application info for an application ID
		/// </summary>
		/// <param name="applicationID">ID associated with the application in question</param>
		/// <returns>Application info for an application ID if exist, null otherwise</returns>
		[OperationContract]
		Application GetApplicationInfo(Guid applicationID);
	}
}
