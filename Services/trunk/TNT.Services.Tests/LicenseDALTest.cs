using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.ServiceModel;
using System.Text.RegularExpressions;
using TNT.Services.DAL;
using TNT.Services.Objects;
using TNT.Services.Objects.Faults;

namespace TNT.Services.Tests
{
	[TestClass]
	public class LicenseDALTest : LicenseDAL
	{
		[TestMethod]
		public void GetNewLicenseTest_NullUser()
		{
			try
			{
				GetNewLicense(null, Guid.Empty);
				Assert.Fail("FaultException<InvalidUserFault> expected");
			}
			catch (FaultException<InvalidUserFault> fault)
			{
				Assert.AreEqual("User not set to instance of an object", fault.Reason.ToString());
				Assert.AreEqual("A user must be specified", fault.Detail.Description);
			}
		}

		[TestMethod]
		public void GetNewLicenseTest_NoName()
		{
			try
			{
				GetNewLicense(new User(), Guid.Empty);
				Assert.Fail("FaultException<InvalidUserFault> expected");
			}
			catch (FaultException<InvalidUserFault> fault)
			{
				Assert.AreEqual("Name property missing", fault.Reason.ToString());
				Assert.AreEqual("User name must be specified", fault.Detail.Description);
			}
		}

		[TestMethod]
		public void GetNewLicenseTest_InvalidAppID()
		{
			try
			{
				User user = new User() { Name = "Steve Tripp" };
				GetNewLicense(user, Guid.Empty);
				Assert.Fail("FaultException<InvalidUserFault> expected");
			}
			catch (FaultException<InvalidApplicationFault> fault)
			{
				Assert.AreEqual("Invalid application ID", fault.Reason.ToString());
				Assert.AreEqual("The application ID is not valid", fault.Detail.Description);
			}
		}

		[TestMethod]
		public void GetNewLicenseTest()
		{
			DateTime now = DateTime.Now;

			User user = new User()
			{
				Address = "Address",
				City = "City",
				EmailAddress = "EmailAddress",
				Name = "Name",
				PhoneNumber = "PhoneNumber",
				State = "State",
				Zip = "Zip"
			};

			Guid guid = new Guid("5FD3560A-0EFD-4E48-9DF9-C4FA3F44BA15");

			License license = GetNewLicense(user, guid);

			Assert.IsTrue(Regex.IsMatch(license.Key, "[A-Z]{4}(-[A-Z]{4}){4}"));
			Assert.AreEqual(guid, license.Application.ID);
			Assert.AreEqual("Name", license.IssuedTo.Name);
			Assert.AreEqual("Address", license.IssuedTo.Address);
			Assert.AreEqual("City", license.IssuedTo.City);
			Assert.AreEqual("EmailAddress", license.IssuedTo.EmailAddress);
			Assert.AreEqual("PhoneNumber", license.IssuedTo.PhoneNumber);
			Assert.AreEqual("State", license.IssuedTo.State);
			Assert.AreEqual("Zip", license.IssuedTo.Zip);
			Assert.IsTrue(license.IssuedOn > now);
		}

		[TestMethod]
		public void GetLicenseTest()
		{
			License license = GetLicense("NSKA-CNWG-SNVB-LRJF-AUOG", new Guid("9F285BEF-D895-46AB-A986-DF1F8167AD7E"));
		}
	}
}
