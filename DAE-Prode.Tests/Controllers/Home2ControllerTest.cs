using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using DAE_Prode;
using DAE_Prode.Controllers;
using NUnit.Mocks;
using NUnit.Framework;

namespace DAE_Prode.Tests.Controllers
{
    [TestFixture]
    public class Home2ControllerTest
    {
        [Test]
        public void Index()
        {
            // Arrange
            Home2Controller controller = new Home2Controller();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.AreEqual("Welcome to ASP.NET MVC!", result.ViewBag.Message);
        }

        [Test]
        public void About()
        {
            // Arrange
            Home2Controller controller = new Home2Controller();

            // Act
            ViewResult result = controller.About() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
