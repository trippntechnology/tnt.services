using System;
using System.Data.Common;
using System.Text;
using TNT.Data.Tools;
using TNT.Services.Objects;

namespace TNT.Services.DAL
{
	/// <summary>
	/// Data abstraction layer for Application records
	/// </summary>
	public class ApplicationDAL : DALBase
	{
		/// <summary>
		/// Gets the application record associated with the application ID
		/// </summary>
		/// <param name="appid">Application ID</param>
		/// <returns>Application record associated with the application ID</returns>
		public static Application GetApplication(Guid appid)
		{
			using (QueryHelper qh = CreateQueryHelper())
			{
				StringBuilder sql = new StringBuilder();

				sql.AppendFormat("select * from Applications where upper(ApplicationID) = upper('{0}')", appid.ToString());

				return qh.ExecuteQuery<Application>(sql.ToString(), System.Data.CommandType.Text, null, dr =>
					{
						Application appInfo = null;

						if (dr.Read())
						{
							appInfo = FillApplication(dr);
						}

						return appInfo;
					});
			}
		}

		/// <summary>
		/// Fills out an Application
		/// </summary>
		/// <param name="dr">Data reader</param>
		/// <returns>ApplicationInfo object representing the results in the data reader</returns>
		public static Application FillApplication(DbDataReader dr)
		{
			Application appInfo = new Application()
			{
				ID = (Guid)dr.GetGuid("ApplicationID"),
				Name = dr.GetString("Name"),
				Version = dr.GetString("Version"),
				URL = new Uri(dr.GetString("URL"))
			};

			return appInfo;
		}
	}
}
