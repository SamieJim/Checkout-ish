using AutoMapper;
using PaymentRetrieve.DTOs;
using PaymentRetrieve.Models;

namespace PaymentRetrieve.Profiles{
    class PaymentProfile : Profile
    {
        public PaymentProfile(){
            CreateMap<Transaction, TransactionReadDTO>();
        }
    }
}