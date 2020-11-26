using System.Collections.Generic;
using AutoMapper;
using Payment.Repositories;
using Payment.Models;
using Microsoft.AspNetCore.Mvc;
using Payment.DTOs;
using System.Net.Http;
using System;
using System.Threading.Tasks;
using MockBank.Controllers;
using MockBank.Models;

namespace Payment.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentRepo _repository;
        private readonly IMapper _mapper;
        private readonly BankController _mockBank = new BankController();

        public PaymentController(IPaymentRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpPost]
        public ActionResult CreateTransaction(TransactionCreateDTO obj)
        {
            var transactionModel = _mapper.Map<Transaction>(obj);
            
            try{
                transactionModel.Status = Status.Pending;
                _repository.CreateTransaction(transactionModel);
                _repository.SaveChanges();

                var response = _mockBank.PaySuccess();
                _repository.UpdateTransactionStatus(transactionModel);
                _repository.SaveChanges();

                transactionModel.Status = (Status)(response.Value as BankResponse).Status;
                _repository.UpdateTransactionStatus(transactionModel);
                _repository.SaveChanges();
                
                return transactionModel.Status == Status.Successful ? response : BadRequest("Payment unsuccessful.");
            }
            catch{
                transactionModel.Status = Status.Failed;
                _repository.CreateTransaction(transactionModel);
                _repository.SaveChanges();
                return BadRequest("Payment unsuccessful.");
            }
        }

    }
}