using Microsoft.VisualStudio.TestTools.UnitTesting;
using TNT.Services.DAL;

namespace TNT.Services.Tests
{
	[TestClass]
	public class UserDALTests : UserDAL
	{
		//[Test]
		//public void InsertUserTest()
		//{
		//  User user = new User()
		//  {
		//    Address = "Address",
		//    ApplicationID = new Guid("9F285BEF-D895-46AB-A986-DF1F8167AD7E"),
		//    City = "City",
		//    EmailAddress = "local@domain.com",
		//    Name = "Name",
		//    PhoneNumber = "PhoneNumber",
		//    State = "State",
		//    Zip = "Zip"
		//  };

		//  user = RegisterUser(user);

		//  Assert.AreEqual(0, user.RowID);

		//  User newUser = GetUser(user.LicenseKey);

		//  Assert.AreEqual(user.Address, newUser.Address);
		//  Assert.AreEqual(user.ApplicationID, newUser.ApplicationID);
		//  Assert.AreEqual(user.City, newUser.City);
		//  Assert.AreEqual(user.EmailAddress, newUser.EmailAddress);
		//  Assert.AreEqual(user.LicenseKey, newUser.LicenseKey);
		//  Assert.AreEqual(user.Name, newUser.Name);
		//  Assert.AreEqual(user.PhoneNumber, newUser.PhoneNumber);
		//  Assert.AreEqual(user.State, newUser.State);
		//  Assert.AreEqual(user.Zip, newUser.Zip);
		//}

		//[Test]
		//public void GetNullUserTest()
		//{
		//  Assert.IsNull(GetUser("Bogus License"));
		//}

	}
}
