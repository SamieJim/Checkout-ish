using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using Gateway.Controllers;
using Gateway.Models;
using Payment.Repositories;
using Microsoft.Extensions.Logging;

namespace Tests.GatewayTests
{
    public class GatewayControllerTests
    {

        #region snippet_Index_ReturnsAViewResult
        [Fact]
        public void Index_ReturnsAViewResult()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<GatewayController>>();
            ILogger<GatewayController> logger = mockLogger.Object;
            var controller = new GatewayController(logger);

            // Act
            var result = controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Transaction>>(
                viewResult.ViewData.Model);
        }
        #endregion

        #region snippet_ModelState_Valid
        [Fact]
        public void IndexPost_ReturnsARedirectWithMessage_WhenModelStateIsValid()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<GatewayController>>();
            ILogger<GatewayController> logger = mockLogger.Object;
            var controller = new GatewayController(logger);

            Card card = new Card(){
                CardNumber = "4590876501283434",
                NameOnCard = "Test Dude",
                ExpiryDate = DateTime.Now.AddDays(300),
                Cvv = 700
            };
            var newTransaction = new Transaction()
            {
                Amount = 1,
                Card = card
            };

            // Act
            var result = controller.Pay(newTransaction);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult);
            Assert.Equal("Index", viewResult.ViewName);
            Assert.Equal("Success, please take note of your transaction ID: ", viewResult.ViewData["Message"].ToString().Substring(0, 49));
        }
        #endregion


    }
}
