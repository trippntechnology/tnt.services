using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TNT.Services.Service;

#nullable enable

namespace Tests
{
	[TestClass]
	public class ExtensionsTests
	{
		[TestMethod]
		public void Extensions_ToVersion_Invalid()
		{
			var defaultVersion = new Version();
			string? version = null;
			Assert.AreEqual(defaultVersion, version.ToVersion());
			Assert.AreEqual(defaultVersion, "".ToVersion());
			Assert.AreEqual(defaultVersion, "1".ToVersion());
			Assert.AreEqual(defaultVersion, "1.1.1.1.1".ToVersion());
		}

		[TestMethod]
		public void Extensions_ToVersion_Valid()
		{
			var v1 = new Version("1.1");
			var v2 = new Version("1.1.1");
			var v3 = new Version("1.1.1.1");
			Assert.AreEqual(v1, "1.1".ToVersion());
			Assert.AreEqual(v2, "1.1.1".ToVersion());
			Assert.AreEqual(v3, "1.1.1.1".ToVersion());
		}
	}
}
