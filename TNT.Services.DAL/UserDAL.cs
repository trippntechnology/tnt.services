using System.Data.Common;
using TNT.Data.Tools;
using TNT.Services.Objects;

namespace TNT.Services.DAL
{
	/// <summary>
	/// Data abstraction layer for User records
	/// </summary>
	public class UserDAL : DALBase
	{
		/// <summary>
		/// Creates a user object with the data contained in the data reader
		/// </summary>
		/// <param name="dr">Data reader</param>
		/// <returns>User representing the data</returns>
		public static User FillUser(DbDataReader dr)
		{
			return new User()
			{
				Address = dr.GetString("Address"),
				City = dr.GetString("City"),
				EmailAddress = dr.GetString("EmailAddress"),
				Name = dr.GetString("Name"),
				PhoneNumber = dr.GetString("PhoneNumber"),
				State = dr.GetString("State"),
				Zip = dr.GetString("Zip")
			};
		}
	}
}
