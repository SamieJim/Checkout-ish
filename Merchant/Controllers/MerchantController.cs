using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Merchant.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Merchant.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class MerchantController : Controller
    {
        private readonly ILogger<MerchantController> _logger;
        public MerchantController(ILogger<MerchantController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet ("{id}")]
        [Route("GetPayment")]
        public IActionResult GetPayment(string transactionId)
        {
            if(String.IsNullOrEmpty(transactionId)){
                ViewData.Add("Message", "Id must have a value!");
                return View("Index");
            }
            Guid id;
            try{
                id = Guid.Parse(transactionId);
            }
            catch{
                ViewData.Add("Message", "Id must be a GUID!");
                return View("Index");
            }
            HttpResponseMessage response = new HttpResponseMessage(){StatusCode = HttpStatusCode.BadRequest};
            
            // local dev environment so trust certs. See considerations.
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            
            using (HttpClient client = new HttpClient(clientHandler))
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string uri = "https://checkout-ish_envoygateway_1:10000/r/" + id;
                response = client.GetAsync(uri).Result; 
            }  
            
            if(response.IsSuccessStatusCode)
            {
                try{
                    PaymentResponse result = JsonConvert.DeserializeObject<PaymentResponse>(response.Content.ReadAsStringAsync().Result);
                    ViewData.Add("Message", "Payment status of " + "£" + result.Amount + " to " + result.CardNumber + " is " + result.Status);
                }
                catch{
                    ViewData.Add("Message", "Could not retrieve payment");
                }
            }
            else{
                ViewData.Add("Message", "Could not retrieve payment");
            }
            return View("Index");
        }
        
    }
}