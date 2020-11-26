using AutoMapper;
using Payment.DTOs;
using Payment.Models;

namespace Payment.Profiles{
    class PaymentProfile : Profile
    {
        public PaymentProfile(){
            CreateMap<TransactionCreateDTO, Transaction>();
        }
    }
}