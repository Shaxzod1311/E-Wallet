using AutoMapper;
using E_Wallet.Domain.Models;
using E_Wallet.Service.DTOs;

namespace E_Wallet.Extensions
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<WalletDTO, Wallet>().ReverseMap();

            CreateMap<Transaction, TransactionDTO>().ReverseMap();

        }
    }
}