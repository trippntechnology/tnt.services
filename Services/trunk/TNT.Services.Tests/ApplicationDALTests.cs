using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TNT.Services.DAL;
using TNT.Services.Objects;

namespace TNT.Services.Tests
{
	[TestClass]
	public class ApplicationDALTests : ApplicationDAL
	{
		[TestMethod]
		public void GetApplicationInfoTest()
		{
			Guid guid = new Guid("5fd3560a-0efd-4e48-9df9-c4fa3f44ba15");
			Assert.IsNull(GetApplication(Guid.Empty));
			Application appInfo = GetApplication(guid);

			Assert.IsNotNull(appInfo);

			Assert.AreEqual(guid, appInfo.ID);
			Assert.AreEqual("Landscape Sprinkler Designer", appInfo.Name.Trim());
			Assert.AreEqual("3.3.27.36", appInfo.Version.Trim());
			Assert.AreEqual(new Uri("http://landscapesprinklerdesign.com/downloads/lsdsetup_3.3.27.36.exe"), appInfo.URL);
		}
	}
}
