using TNT.Data.Tools;

namespace TNT.Services.DAL
{
	/// <summary>
	/// Base class for all DAL classes
	/// </summary>
	public class DALBase
	{
		/// <summary>
		/// Creates QueryHelper used by DAL classes. 
		/// </summary>
		/// <returns>TNT.Data.Tools.QueryHelper</returns>
		public static QueryHelper CreateQueryHelper()
		{
			return new QueryHelper("SQLServer");
		}
	}
}
