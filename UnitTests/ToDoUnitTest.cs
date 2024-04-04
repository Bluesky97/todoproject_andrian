using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using todoproject_andrian.Controllers;
using todoproject_andrian.Models;
using Assert = NUnit.Framework.Assert;

namespace todoproject_andrian.UnitTests
{
    public class ToDoUnitTest
    {
        [TestClass]
        public class HomeControllerTests
        {
            [TestMethod]
            public void Index_ReturnsViewResult()
            {
                // Arrange
                var loggerMock = new Mock<ILogger<HomeController>>();
                var controller = new HomeController(loggerMock.Object);

                // Act
                var result = controller.Index();

                // Assert
                Assert.That(result is ViewResult);
            }

            [TestMethod]
            public void PopulateForm_ReturnsJsonResult()
            {
                // Arrange
                var loggerMock = new Mock<ILogger<HomeController>>();
                var controller = new HomeController(loggerMock.Object);
                int id = 1;

                // Act
                var result = controller.PopulateForm(id);

                // Assert
                Assert.That(result is JsonResult);
            }
        }
    }
}
