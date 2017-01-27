using TNT.Data.Tools;
using TNT.Utilities;

namespace TNT.Services.DAL
{
	/// <summary>
	/// Collection of methods related to registration
	/// </summary>
	public class RegistrationDAL : DALBase
	{
		/// <summary>
		/// Gets a unique license key
		/// </summary>
		/// <returns>Unique license key</returns>
		public static string GetUniqueLicenseKey()
		{
			string key = string.Empty;
			bool keyExists = false;

			using (QueryHelper qh = CreateQueryHelper())
			{
				// Generate a key
				do
				{
					key = Registration.GenerateKey(20, 4);
					keyExists = qh.ExecuteQuery<bool>(string.Format("select count(*) count from Licenses where LicenseKey = '{0}'", key), System.Data.CommandType.Text, null, dr =>
						{
							dr.Read();
							long count = (long)dr.GetInt64("count");
							return count > 0;
						});
				}
				while (keyExists);
			}

			return key;
		}
	}
}
