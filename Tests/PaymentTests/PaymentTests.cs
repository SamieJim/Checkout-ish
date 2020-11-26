using Xunit;
using Tests.MockTransactDb;
using System;
using Moq;
using Microsoft.Extensions.Logging;
using PaymentRetrieve.Controllers;
using AutoMapper;
using Payment.Controllers;
using Microsoft.AspNetCore.Mvc;
using PaymentRetrieve.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;
using Gateway.Models;
using Payment.DTOs;

namespace Tests.PaymentTests
{
    public class PaymentTests{
         
        #region GetreturnsBankResponse
        [Fact]
        public void Get_ReturnsIdAndStatus()
        {
            // Arrange
            var testAmount = 1.0;
            string testId = "6f51d725-9851-4ae9-9c43-c4a8aee2dd20"; 
            Payment.Models.Card card = new Payment.Models.Card(){
                CardNumber = "4590876501283434",
                NameOnCard = "Test Dude",
                ExpiryDate = DateTime.Now.AddDays(300),
                Cvv = 700
            };
            var newTransaction = new Payment.DTOs.TransactionCreateDTO()
            {
                Amount = 1,
                Card = card
            };

            
            var retrieveTransaction = new PaymentRetrieve.Models.Transaction(){
                Id = new Guid(testId),
                Status = PaymentRetrieve.Models.Status.Successful,
                Amount = testAmount,
                CardNumber = newTransaction.Card.CardNumber,
                Currency = System.Globalization.NumberFormatInfo.CurrentInfo.CurrencySymbol
            };

            var mockRepo = new Mock<IMockTransactionRepo>();
            mockRepo.Setup(repo => repo.GetTransactionById(new Guid(testId)))
                .Returns(retrieveTransaction);
            
            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(x => x.Map<TransactionCreateDTO>(It.IsAny<Payment.Models.Transaction>()))
                                .Returns((TransactionCreateDTO tcdto) =>
                                {
                                    var transaction = new TransactionCreateDTO()
                                    {
                                        Amount = tcdto.Amount,
                                        Card = tcdto.Card
                                    };

                                    return transaction;
                
                                });
            var controllerRetrieve = new PaymentRetrieveController(mockRepo.Object, mockMapper.Object);
            var controllerPay = new PaymentController(mockRepo.Object, mockMapper.Object);

            // Act
            var result = controllerPay.CreateTransaction(newTransaction);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnTrans = Assert.IsType<MockBank.Models.BankResponse>(okResult.Value);
            mockRepo.Verify();
            Assert.Equal(2, returnTrans.Status);
        }
        #endregion

        #region Get_BadBankResponse
        [Fact]
        public void Get_NotFound()
        {
            // Arrange
            var testAmount = 1.0;
            string testId = "6f51d725-9851-4ae9-9c43-c4a8aee2dd20"; 
            Payment.Models.Card card = new Payment.Models.Card(){
                CardNumber = "4590876501283434",
                NameOnCard = "Test Dude",
                ExpiryDate = DateTime.Now.AddDays(300),
                Cvv = 700
            };
            var newTransaction = new Payment.DTOs.TransactionCreateDTO()
            {
                Amount = 1,
                Card = card
            };

            
            var retrieveTransaction = new PaymentRetrieve.Models.Transaction(){
                Id = new Guid(testId),
                Status = PaymentRetrieve.Models.Status.Successful,
                Amount = testAmount,
                CardNumber = newTransaction.Card.CardNumber,
                Currency = System.Globalization.NumberFormatInfo.CurrentInfo.CurrencySymbol
            };

            var mockRepo = new Mock<IMockTransactionRepo>();
            mockRepo.Setup(repo => repo.GetTransactionById(new Guid(testId)))
                .Returns(retrieveTransaction);
            
            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(x => x.Map<TransactionCreateDTO>(It.IsAny<Payment.Models.Transaction>()))
                                .Returns((TransactionCreateDTO tcdto) =>
                                {
                                    var transaction = new TransactionCreateDTO()
                                    {
                                        Amount = tcdto.Amount,
                                        Card = tcdto.Card
                                    };

                                    return transaction;
                
                                });
            var controllerRetrieve = new PaymentRetrieveController(mockRepo.Object, mockMapper.Object);
            var controllerPay = new PaymentController(mockRepo.Object, mockMapper.Object);

            // Act
            controllerPay.CreateTransaction(newTransaction);
            var result = controllerRetrieve.GetTransactionById("c7d9c980-9747-4ff9-a6b8-ad9dc36dc080");

            // Assert
            var okResult = Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void Get_WithBadModel_ReturnsModelValidationErrors()
        {
            // Arrange
            Payment.Models.Card card = new Payment.Models.Card(){
                CardNumber = "Nonsense",
                NameOnCard = "Subject 0123B",
                ExpiryDate = DateTime.Now.AddDays(-300),
                Cvv = 7000
            };
            var newTransaction = new Payment.DTOs.TransactionCreateDTO()
            {
                //Negative 1 should cause error
                Amount = -1,
                Card = card
            };

            var mockRepo = new Mock<IMockTransactionRepo>();
            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(x => x.Map<TransactionCreateDTO>(It.IsAny<Payment.Models.Transaction>()))
                                .Returns((TransactionCreateDTO tcdto) =>
                                {
                                    var transaction = new TransactionCreateDTO()
                                    {
                                        Amount = tcdto.Amount,
                                        Card = tcdto.Card
                                    };

                                    return transaction;
                
                                });
            var controllerPay = new PaymentController(mockRepo.Object, mockMapper.Object);

            // Act
            var result = controllerPay.CreateTransaction(newTransaction);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void GetPayment_RespondsWithSuccess()
        {
            //Arrange
            Payment.Models.Card card = new Payment.Models.Card(){
                CardNumber = "Nonsense",
                NameOnCard = "Subject 0123B**--&&^^//'';;YE33555",
                ExpiryDate = DateTime.Now.AddDays(-300),
                Cvv = 7000
            };
            var newTransaction = new Payment.DTOs.TransactionCreateDTO()
            {
                //Negative 1 should cause error
                Amount = -1,
                Card = card
            };

            HttpResponseMessage response = new HttpResponseMessage();
            
            // Add
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            using (HttpClient client = new HttpClient(clientHandler))
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string uri = "https://checkout-ish_envoygateway_1:10000/p";
                var content = new StringContent(JsonConvert.SerializeObject(newTransaction), Encoding.UTF8, "application/json");
                response = client.PostAsync(uri, content).Result; 
            }  

            var result = response.Content.ReadAsStringAsync().Result;

            //Asserts
            Assert.False(response.IsSuccessStatusCode);
            Assert.Contains(result, "Your card has expired.");
            Assert.Contains(result, "Amount must be more than 0!");
            Assert.Contains(result, "Invalid card number format.");
            Assert.Contains(result, "Please enter a valid name.");
            Assert.Contains(result, "Please enter a valid CVV.");

        }
        #endregion
    }
}        
        
       