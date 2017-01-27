using System;
using System.ServiceModel;
using System.Text;
using TNT.Data.Tools;
using TNT.Services.Objects;
using TNT.Services.Objects.Faults;

namespace TNT.Services.DAL
{
	/// <summary>
	/// Data abstraction for Authorization records
	/// </summary>
	public class AuthorizationDAL : DALBase
	{
		/// <summary>
		/// Gets an authorization key for a given hardware ID, application ID, and license key
		/// </summary>
		/// <param name="hardwareID">Hardware ID</param>
		/// <param name="appID">Application ID</param>
		/// <param name="licenseKey">License key</param>
		/// <returns>Authorization key if parameters are valid, string.Empty otherwise</returns>
		public static string GetAuthorizationKey(string hardwareID, Guid appID, string licenseKey)
		{
			string authKey = string.Empty;

			if (string.IsNullOrEmpty(hardwareID))
			{
				throw new FaultException<InvalidParameterFault>(new InvalidParameterFault("Hardware ID is missing"), "Missing parameter");
			}
			else if (string.IsNullOrEmpty(licenseKey))
			{
				throw new FaultException<InvalidParameterFault>(new InvalidParameterFault("License key is missing"), "Missing parameter");
			}

			Application app = ApplicationDAL.GetApplication(appID);

			if (app == null)
			{
				throw new FaultException<InvalidApplicationFault>(new InvalidApplicationFault("The application ID is not valid"), "Invalid application ID");
			}

			License license = LicenseDAL.GetLicense(licenseKey, appID);

			if (license == null)
			{
				throw new FaultException<InvalidParameterFault>(new InvalidParameterFault("The license key is not valid for this application"), "Invalid license key");
			}

			if (license != null)
			{
				// Create Authorization key
				string seed = string.Concat(hardwareID, appID.ToString(), licenseKey);
				authKey = Utilities.Registration.GenerateSHA1Hash(seed);
			}

			InsertAuthorization(authKey, licenseKey, hardwareID);

			return authKey;
		}

		/// <summary>
		/// Creates an authorization record
		/// </summary>
		/// <param name="authKey">Authentication key</param>
		/// <param name="licenseKey">License key</param>
		/// <param name="hardwareID">Hardware ID</param>
		private static void InsertAuthorization(string authKey, string licenseKey, string hardwareID)
		{
			using (QueryHelper qh = CreateQueryHelper())
			{
				StringBuilder query = new StringBuilder();

				query.AppendFormat("insert into Authorizations values('{0}','{1}','{2}','{3}')", authKey, licenseKey, hardwareID, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));

				qh.ExecuteQuery(query.ToString(), System.Data.CommandType.Text);
			}
		}
	}
}
