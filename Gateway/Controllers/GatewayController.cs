using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Gateway.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Gateway.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class GatewayController : Controller
    {
        private readonly ILogger<GatewayController> _logger;
        public GatewayController(ILogger<GatewayController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("Pay")]
        public IActionResult Pay([FromForm] Transaction transaction)
        {
            HttpResponseMessage response = new HttpResponseMessage(){StatusCode = HttpStatusCode.BadRequest};
            
            // local dev environment so trust certs. See considerations.
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            
            using (HttpClient client = new HttpClient(clientHandler))
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string uri = "https://checkout-ish_envoygateway_1:10000/p";
                var content = new StringContent(JsonConvert.SerializeObject(transaction), Encoding.UTF8, "application/json");
                response = client.PostAsync(uri, content).Result; 
            }  
            
            PaymentResponse result = JsonConvert.DeserializeObject<PaymentResponse>(response.Content.ReadAsStringAsync().Result);
            ViewData.Add(
                new KeyValuePair<string, object>("Message", (result.Status + ", please take note of your transaction ID: " + result.Id)));
            
            return View("Index");
        }
        
    }
}