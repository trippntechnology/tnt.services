
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using TNT.Services.Models.Request;
using TNT.Services.Service.Controllers;
using TNT.Services.Service.Data;
using TNT.Services.Service.Models.Entities;

namespace Tests
{
	[TestClass]
	public class V1ControllerTests : ContextDependentTests
	{
		[TestMethod]
		public void V1Controller_TestConnection()
		{
			var mockContext = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
			var sut = new V1Controller(mockContext.Object);
			var result = sut.TestConnection();
			Assert.IsTrue(result.IsSuccess);
			Assert.AreEqual("Congratulations! You have successfully connected.", result.Message);
		}

		[TestMethod]
		public void V1Controller_GetApplicationInfo_InvalidApplicationId()
		{
			var mockContext = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
			mockContext.Setup(m => m.Application).Returns(GetDbSet(Applications));
			mockContext.Setup(m => m.Release).Returns(GetDbSet(Releases));

			var sut = new V1Controller(mockContext.Object);

			var result = sut.GetApplicationInfo(new ApplicationRequest { ApplicationID = 5 });

			Assert.IsFalse(result.IsSuccess);
			Assert.AreEqual("ApplicationNotFoundException: Application ID, 5 does not exist", result.Message);
		}

		[TestMethod]
		public void V1Controller_GetApplicationInfo_NoRelease()
		{
			var mockContext = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
			mockContext.Setup(m => m.Application).Returns(GetDbSet(Applications));
			mockContext.Setup(m => m.Release).Returns(GetDbSet(new List<Release>()));

			var sut = new V1Controller(mockContext.Object);
			var result = sut.GetApplicationInfo(new ApplicationRequest { ApplicationID = 1 });

			Assert.IsFalse(result.IsSuccess);
			Assert.AreEqual("ReleaseNotFoundException: Release associated with application ID, 1, could not be found", result.Message);
		}

		[TestMethod]
		public void V1Controller_GetApplicationInfo_Valid()
		{
			var mockContext = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
			mockContext.Setup(m => m.Application).Returns(GetDbSet(Applications));
			mockContext.Setup(m => m.Release).Returns(GetDbSet(Releases));

			var sut = new V1Controller(mockContext.Object);

			var result = sut.GetApplicationInfo(new ApplicationRequest { ApplicationID = 1 });

			Assert.AreEqual(Applications[0].Name, result.Name);
			Assert.AreEqual(Releases[0].Version, result.ReleaseVersion);
			Assert.AreEqual(Releases[0].Date, result.ReleaseDate);
			Assert.AreEqual(Releases[0].ID, result.ReleaseID);
			Assert.IsTrue(result.IsSuccess);
			Assert.IsTrue(string.IsNullOrWhiteSpace(result.Message));
		}

		[TestMethod]
		public void V1Controller_GetRelease_InvalidReleaseID()
		{
			var mockContext = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
			mockContext.Setup(m => m.Application).Returns(GetDbSet(Applications));
			mockContext.Setup(m => m.Release).Returns(GetDbSet(new List<Release>()));

			var sut = new V1Controller(mockContext.Object);
			var result = sut.GetRelease(new ReleaseRequest { ReleaseId = 1 });

			Assert.IsFalse(result.IsSuccess);
			Assert.AreEqual("ReleaseNotFoundException: Release ID, 1, could not be found", result.Message);
		}

		[TestMethod]
		public void V1Controller_GetRelease_Valid()
		{
			var mockContext = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
			mockContext.Setup(m => m.Application).Returns(GetDbSet(Applications));
			mockContext.Setup(m => m.Release).Returns(GetDbSet(Releases));

			var sut = new V1Controller(mockContext.Object);
			var result = sut.GetRelease(new ReleaseRequest { ReleaseId = 1 });

			Assert.IsTrue(result.IsSuccess);
			Assert.AreEqual(Releases[0].Date, result.ReleaseDate);
			Assert.AreEqual(Convert.ToBase64String(Releases[0].Package), result.Package);
			Assert.AreEqual(Releases[0].FileName, result.FileName);
		}
	}
}
