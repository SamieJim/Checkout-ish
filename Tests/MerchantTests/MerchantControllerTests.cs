using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using Merchant.Controllers;
using Merchant.Models;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace Tests.MerchantTests
{
    public class MerchantControllerTests
    {

        #region snippet_Index_ReturnsAViewResult
        [Fact]
        public void Index_ReturnsAViewResult()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<MerchantController>>();
            ILogger<MerchantController> logger = mockLogger.Object;
            var controller = new MerchantController(logger);

            // Act
            var result = controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
        }
        #endregion

        #region api_responsive
        [Fact]
        public void GetPayment_RespondsWithSuccess()
        {
            HttpResponseMessage response = new HttpResponseMessage();
            
            // local dev environment so trust certs. See considerations.
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            using (HttpClient client = new HttpClient(clientHandler))
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string uri = "https://checkout-ish_merchant_1:5001/Merchant/GetPayment/c7ef00bd-53ef-4828-0fd4-08d88ff970d2";
                response = client.GetAsync(uri).Result; 
            }  

            Assert.True(response.IsSuccessStatusCode);

        }

        [Fact]
        public void GetPayment_CorrectMessageSet()
        {
            HttpResponseMessage response = new HttpResponseMessage();
            
            // local dev environment so trust certs. See considerations.
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            using (HttpClient client = new HttpClient(clientHandler))
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string uri = "https://checkout-ish_merchant_1:5001/Merchant/GetPayment/c7ef00bd-53ef-4828-0fd4-08d88ff970d2";
                response = client.GetAsync(uri).Result; 
            }  
            ViewResult result = JsonConvert.DeserializeObject<ViewResult>(response.Content.ReadAsStringAsync().Result);

            Assert.Contains(" is Succesful"
                            , result.ViewData["Message"].ToString());

        }
        #endregion

        #region api_expect_fails
        [Fact]
        public void GetPayment_RespondsWithError_EmptyId(){
            HttpResponseMessage response = new HttpResponseMessage();
            
            // local dev environment so trust certs. See considerations.
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            using (HttpClient client = new HttpClient(clientHandler))
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string uri = "https://checkout-ish_merchant_1:5001/Merchant/GetPayment/";
                response = client.GetAsync(uri).Result; 
            }  

            Assert.False(response.IsSuccessStatusCode);
        }
        #endregion


    }
}
