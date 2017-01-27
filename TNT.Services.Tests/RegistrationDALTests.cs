using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.RegularExpressions;
using TNT.Services.DAL;

namespace TNT.Services.Tests
{
	[TestClass]
	public class RegistrationDALTests :RegistrationDAL
	{
		[TestMethod]
		public void GetUniqueLicenseKeyTest()
		{
			string key = GetUniqueLicenseKey();

			Assert.IsTrue(Regex.IsMatch(key, "^[A-Z]{4}(-[A-Z]{4}){4}$"));
		}
	}
}
