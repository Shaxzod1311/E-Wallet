using AutoMapper;
using E_Wallet.Domain.Models;
using E_Wallet.Service.DTOs;

namespace E_Wallet.Service.Mapping
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<Wallet, WalletDTO>().ReverseMap();
        }
    }
}