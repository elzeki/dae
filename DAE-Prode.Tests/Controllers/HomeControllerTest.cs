using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using DAE_Prode;
using DAE_Prode.Controllers;
using DAE_Prode.Models;
using System.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DAE_Prode.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        private PRODEDataContext db = new PRODEDataContext();

        [TestMethod]
        public void Index()
        {            
            // Arrange
            HomeController controllerToTest = new HomeController();

            usuariomodels U = new usuariomodels();
            U.id = 1;
            U.username = "silchu";
            U.password = "silchu1234";

            controllerToTest.HttpContext.Session.Add("userame", U.username);
            controllerToTest.Session.Add("username", U.username);
            controllerToTest.Session.Add("password", "silchu1234");
            controllerToTest.Session.Add("rol","usuario");
            controllerToTest.Session.Add("nombre", "silvia");

            var countSession = controllerToTest.Session.Count;

            if (countSession != 0)
            {

            }

        }

        [TestMethod]
        public void Reglamento()
        {
            // Arrange
            HomeController controller = new HomeController();
            // Act
            ViewResult result = controller.Reglamento() as ViewResult;
            // Assert
            Assert.IsNotNull(result);
        }
        
    }
}