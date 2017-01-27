using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text.RegularExpressions;
using TNT.Services.Contracts;
using TNT.Services.Objects;
using TNT.Utilities;

namespace TNT.Services.Tests
{
	[TestClass]
	public class ServiceTests
	{
		TNTServicesClient m_TNTClient = null;

		[TestInitialize]
		public void Setup()
		{
			try
			{
				m_TNTClient = new TNTServicesClient("BasicHttpBinding_ITNTServicesContracts");
				Assert.IsNotNull(m_TNTClient);
				Assert.IsTrue(m_TNTClient.TestConnectivity());
			}
			catch (Exception ex)
			{
				Assert.Fail(ex.Message);
			}
		}

		[TestMethod]
		public void TestConnectivity()
		{
			Assert.IsTrue(m_TNTClient.TestConnectivity());
		}

		[TestMethod]
		public void RequestLicenseAuthorizationTests()
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

			Guid guid = new Guid("5fd3560a-0efd-4e48-9df9-c4fa3f44ba15");

			License license = m_TNTClient.RequestLicense(user, guid);

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

			string hardwareID = Registration.GetVolumeSerialNumber();
			string authKey = m_TNTClient.GetAuthorizationKey(hardwareID, guid, license.Key);

			Assert.AreEqual(Registration.GenerateSHA1Hash(string.Concat(hardwareID, guid.ToString(), license.Key)), authKey);
			Assert.AreNotEqual(Registration.GenerateSHA1Hash(string.Concat("bogushardwareid", guid.ToString(), license.Key)), authKey);
		}

		[TestMethod]
		public void TestGetUpdateInfo()
		{
			try
			{
				Guid guid = new Guid("5fd3560a-0efd-4e48-9df9-c4fa3f44ba15");
				Application appInfo = m_TNTClient.GetApplicationInfo(guid);

				Assert.IsNotNull(appInfo);
				Assert.AreEqual(guid, appInfo.ID);
				Assert.AreEqual("3.3.27.36", appInfo.Version.Trim());
				Assert.AreEqual(new Uri("http://landscapesprinklerdesign.com/downloads/lsdsetup_3.3.27.36.exe"), appInfo.URL);
			}
			catch (Exception ex)
			{
				Assert.Fail(ex.Message);
			}
		}
	}
}
