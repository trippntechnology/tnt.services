using System;
using System.Data.Common;
using System.ServiceModel;
using System.Text;
using TNT.Data.Tools;
using TNT.Services.Objects;
using Faults = TNT.Services.Objects.Faults;

namespace TNT.Services.DAL
{
	/// <summary>
	/// Data abstraction layer for Licenses
	/// </summary>
	public class LicenseDAL : DALBase
	{
		/// <summary>
		/// Gets a new license for the given user and application
		/// </summary>
		/// <param name="user">User</param>
		/// <param name="appID">Applicaton ID</param>
		/// <returns>License for the given user and application</returns>
		public static License GetNewLicense(User user, Guid appID)
		{
			if (user == null)
			{
				throw new FaultException<Faults.InvalidUserFault>(new Faults.InvalidUserFault("A user must be specified"), "User not set to instance of an object");
			}
			else if (string.IsNullOrEmpty(user.Name))
			{
				throw new FaultException<Faults.InvalidUserFault>(new Faults.InvalidUserFault("User name must be specified"), "Name property missing");
			}

			// Get the application
			Application application = ApplicationDAL.GetApplication(appID);

			if (application == null)
			{
				throw new FaultException<Faults.InvalidApplicationFault>(new Faults.InvalidApplicationFault("The application ID is not valid"), "Invalid application ID");
			}

			License license = new License()
			{
				Application = application,
				IssuedOn = DateTime.Now,
				IssuedTo = user,
				Key = RegistrationDAL.GetUniqueLicenseKey()
			};

			InsertLicense(license);

			return license;
		}

		/// <summary>
		/// Inserts a license into the Licenses table
		/// </summary>
		/// <param name="license">License</param>
		/// <returns>Number of rows effected</returns>
		public static void InsertLicense(License license)
		{
			using (QueryHelper qh = CreateQueryHelper())
			{
				StringBuilder query = new StringBuilder();

				query.Append("insert into Licenses values(");
				query.AppendFormat("'{0}',", license.Key);
				query.AppendFormat("'{0}',", license.Application.ID.ToString());
				query.AppendFormat("'{0}',", license.IssuedTo.Name);
				query.AppendFormat("'{0}',", license.IssuedTo.EmailAddress);
				query.AppendFormat("'{0}',", license.IssuedTo.Address);
				query.AppendFormat("'{0}',", license.IssuedTo.City);
				query.AppendFormat("'{0}',", license.IssuedTo.State);
				query.AppendFormat("'{0}',", license.IssuedTo.Zip);
				query.AppendFormat("'{0}',", license.IssuedTo.PhoneNumber);
				query.AppendFormat("'{0}');", license.IssuedOn.ToString("yyyy-MM-dd HH:mm:ss.fff"));

				qh.ExecuteQuery(query.ToString(), System.Data.CommandType.Text);
			}
		}

		/// <summary>
		/// Gets the license associated with the license key and application ID
		/// </summary>
		/// <param name="licenseKey">License key</param>
		/// <param name="appID">Application ID</param>
		/// <returns>License associated with the license key and application ID</returns>
		public static License GetLicense(string licenseKey, Guid appID)
		{
			using (QueryHelper qh = CreateQueryHelper())
			{
				StringBuilder query = new StringBuilder();

				query.AppendLine("select * from");
				query.AppendLine("  Licenses l");
				query.AppendLine("    inner join Applications a on a.ApplicationID = l.ApplicationID");
				query.AppendLine("  where");
				query.AppendFormat("    l.LicenseKey = '{0}' and upper(a.ApplicationID) = upper('{1}')", licenseKey, appID.ToString());

				return qh.ExecuteQuery<License>(query.ToString(), System.Data.CommandType.Text, null, dr =>
					{
						if (dr.Read())
						{
							return FillLicense(dr);
						}

						return null;
					});
			}
		}

		/// <summary>
		/// Creates a License from the data contained in the data reader
		/// </summary>
		/// <param name="dr">Data reader</param>
		/// <returns>License containing data in data reader</returns>
		private static License FillLicense(DbDataReader dr)
		{
			return new License()
			{
				Application = ApplicationDAL.FillApplication(dr),
				IssuedOn = (DateTime)dr.GetDateTime("IssuedOn"),
				IssuedTo = UserDAL.FillUser(dr),
				Key = dr.GetString("LicenseKey")
			};
		}
	}
}
