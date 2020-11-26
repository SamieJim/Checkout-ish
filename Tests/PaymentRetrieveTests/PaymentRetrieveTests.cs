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
using Payment.DTOs;

namespace Tests.PaymentRetrieveTests
{
    public class PaymentRetrieveTest{
         
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
            controllerPay.CreateTransaction(newTransaction);
            var result = controllerRetrieve.GetTransactionById(testId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnTrans = Assert.IsType<PaymentRetrieve.Models.Transaction>(okResult.Value);
            mockRepo.Verify();
            Assert.Equal(1, returnTrans.Amount);
            Assert.Equal("************3434", returnTrans.CardNumber);
            Assert.Equal(Status.Successful, returnTrans.Status);
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
            mockMapper.Setup(x => x.Map<Payment.Models.Transaction, TransactionCreateDTO>(It.IsAny<Payment.Models.Transaction>()))
                                .Returns(newTransaction);
            var controllerRetrieve = new PaymentRetrieveController(mockRepo.Object, mockMapper.Object);
            var controllerPay = new PaymentController(mockRepo.Object, mockMapper.Object);

            // Act
            controllerPay.CreateTransaction(newTransaction);
            var result = controllerRetrieve.GetTransactionById("c7d9c980-9747-4ff9-a6b8-ad9dc36dc080");

            // Assert
            var okResult = Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void Get_WithBadID_ReturnsError()
        {
            // Arrange
            var testAmount = 1.0;
            string testId = "6ee2dd20"; 
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
                Id = new Guid("90188b1a-36ec-4c50-a667-66204d8ba1a9"),
                Status = PaymentRetrieve.Models.Status.Successful,
                Amount = testAmount,
                CardNumber = newTransaction.Card.CardNumber,
                Currency = System.Globalization.NumberFormatInfo.CurrentInfo.CurrencySymbol
            };

            var mockRepo = new Mock<IMockTransactionRepo>();
            mockRepo.Setup(repo => repo.GetTransactionById(new Guid("90188b1a-36ec-4c50-a667-66204d8ba1a9")))
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
            var result = controllerRetrieve.GetTransactionById(testId);

            // Assert
            var badRequest = Assert.IsType<BadRequestResult>(result);
        }
        #endregion
    }
}        
        
       