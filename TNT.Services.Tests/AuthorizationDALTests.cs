using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.ServiceModel;
using TNT.Services.DAL;
using TNT.Services.Objects;
using TNT.Services.Objects.Faults;
using TNT.Utilities;

namespace TNT.Services.Tests
{
	[TestClass]
	public class AuthorizationDALTests : AuthorizationDAL
	{
		[TestMethod]
		public void GetAuthorizationKeyTest_MissingHardwareParam()
		{
			try
			{
				GetAuthorizationKey("", Guid.Empty, "");
				Assert.Fail("FaultException<InvalidParameterFault> expected");
			}
			catch (FaultException<InvalidParameterFault> fault)
			{
				Assert.AreEqual("Hardware ID is missing", fault.Detail.Description);
				Assert.AreEqual("Missing parameter", fault.Reason.ToString());
			}
		}

		[TestMethod]
		public void GetAuthorizationKeyTest_MissingLicenseKeyParam()
		{
			try
			{
				GetAuthorizationKey("HardwareID", Guid.Empty, "");
				Assert.Fail("FaultException<InvalidParameterFault> expected");
			}
			catch (FaultException<InvalidParameterFault> fault)
			{
				Assert.AreEqual("License key is missing", fault.Detail.Description);
				Assert.AreEqual("Missing parameter", fault.Reason.ToString());
			}
		}

		[TestMethod]
		public void GetAuthorizationKeyTest_InvalidAppID()
		{
			try
			{
				GetAuthorizationKey("HardwareID", Guid.Empty, "LicenseKey");
				Assert.Fail("FaultException<InvalidApplicationFault> expected");
			}
			catch (FaultException<InvalidApplicationFault> fault)
			{
				Assert.AreEqual("The application ID is not valid", fault.Detail.Description);
				Assert.AreEqual("Invalid application ID", fault.Reason.ToString());
			}
		}

		[TestMethod]
		public void GetAuthorizationKeyTest_InvalidLicenseKey()
		{
			try
			{
				GetAuthorizationKey("HardwareID", new Guid("5FD3560A-0EFD-4E48-9DF9-C4FA3F44BA15"), "InvalidKey");
				Assert.Fail("FaultException<InvalidApplicationFault> expected");
			}
			catch (FaultException<InvalidParameterFault> fault)
			{
				Assert.AreEqual("The license key is not valid for this application", fault.Detail.Description);
				Assert.AreEqual("Invalid license key", fault.Reason.ToString());
			}
		}

		[TestMethod]
		public void GetAuthorizationKeyTest()
		{
			User user = new User()
			{
				Address = "Address",
				City = "City",
				EmailAddress = "EmailAddress",
				Name = "GetAuthorizationKeyTestMethod",
				PhoneNumber = "PhoneNumber",
				State = "State",
				Zip = "Zip"
			};

			string hardwareID = "HardwareID";
			Guid guid = new Guid("5FD3560A-0EFD-4E48-9DF9-C4FA3F44BA15");
			License license = LicenseDAL.GetNewLicense(user, guid);

			string authKey = GetAuthorizationKey(hardwareID, guid, license.Key);

			Assert.AreEqual(Registration.GenerateSHA1Hash(string.Concat(hardwareID, guid.ToString(), license.Key)), authKey);
		}
	}
}
