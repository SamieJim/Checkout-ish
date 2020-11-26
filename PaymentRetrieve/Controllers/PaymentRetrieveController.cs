using System.Collections.Generic;
using AutoMapper;
using PaymentRetrieve.Repositories;
using PaymentRetrieve.Models;
using Microsoft.AspNetCore.Mvc;
using PaymentRetrieve.DTOs;
using System.Net.Http;
using System;
using System.Threading.Tasks;

namespace PaymentRetrieve.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class PaymentRetrieveController : ControllerBase
    {
        private readonly IPaymentRepository _repository;
        private readonly IMapper _mapper;
        public PaymentRetrieveController(IPaymentRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet("{id}", Name="GetTransactionById")]
        public ActionResult <TransactionReadDTO> GetTransactionById(string id)
        {
            Guid trancastionId;
            try{
                trancastionId = Guid.Parse(id);
            }
            catch{
                return BadRequest("ID must be of type GUID!");
            }
            var transactionItem = _repository.GetTransactionById(trancastionId);
            var result = _mapper.Map<TransactionReadDTO>(transactionItem);
            if(transactionItem != null)
            {
                return Ok(result);
            }
            return NotFound();
        }

    }
}