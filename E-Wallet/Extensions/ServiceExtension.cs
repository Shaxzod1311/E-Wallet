﻿using AutoMapper;
using E_Wallet.Data.Data;
using E_Wallet.Data.IRepositories;
using E_Wallet.Data.Repositories;
using E_Wallet.Domain.Common;
using E_Wallet.Service.Helpers;
using E_Wallet.Service.Interfaces;
using E_Wallet.Service.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace E_Wallet.Extensions
{
    public static class ServiceExtension
    {
        public static void AddCustomServices(this IServiceCollection services)
        {

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IWalletRepository, WalletRepository>();
            services.AddScoped<ITrasnactionService, TransactionService>();
            services.AddScoped<IWalletService, WalletService>();
            services.AddScoped<IUserService, UserService>();

            services.AddScoped<HttpContextHelper>();
            services.AddScoped<IdentifyUser>();
            services.AddScoped<WalletDbSeed>();

            var mapperConfig = new MapperConfiguration(mc => {
                mc.AddProfile(new AutoMapperConfig());
                mc.AllowNullCollections = true;
            });

            IMapper mapper = mapperConfig.CreateMapper();

            services.AddSingleton(mapper);

            services.AddHttpContextAccessor();
        }
    }
}
