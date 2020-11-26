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
            Guid id = Guid.Parse(transactionId);
            if(id == null){
                ViewBag.Message = "Failed to retrieve!";
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
            
            PaymentResponse result = JsonConvert.DeserializeObject<PaymentResponse>(response.Content.ReadAsStringAsync().Result);
            ViewData.Add("Message", "Payment status of " + "£" + result.Amount + " to " + result.CardNumber + " is " + result.Status);
            
            return View("Index");
        }
        
    }
}