using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using TNT.Data.Tools;

namespace TNT.Services.ApplicationDeployer
{
	public static class SQLServer
	{
		public static void AddUpdateApplication(string guid, string name, string version, string url)
		{
			using (QueryHelper qh = new QueryHelper("SQLServer"))
			{
				List<DbParameter> parms = new List<DbParameter>();

				parms.Add(qh.CreateParameter("@guid", DbType.String, guid));
				parms.Add(qh.CreateParameter("@name", DbType.String, name));
				parms.Add(qh.CreateParameter("@version", DbType.String, version));
				parms.Add(qh.CreateParameter("@url", DbType.String, url));

				try
				{
					qh.ExecuteQuery("insert into Applications values(@guid, @name, @version, @url)", CommandType.Text, parms);
				}
				catch
				{
					parms.Clear();
					parms.Add(qh.CreateParameter("@guid", DbType.String, guid));
					parms.Add(qh.CreateParameter("@name", DbType.String, name));
					parms.Add(qh.CreateParameter("@version", DbType.String, version));
					parms.Add(qh.CreateParameter("@url", DbType.String, url));

					qh.ExecuteQuery("update Applications set Name = @name, Version = @version, URL = @url where ApplicationID = @guid", CommandType.Text, parms);
				}
			}
		}
	}
}
