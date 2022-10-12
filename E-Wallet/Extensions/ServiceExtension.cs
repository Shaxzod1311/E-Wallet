using AutoMapper;
using E_Wallet.Data.IRepositories;
using E_Wallet.Data.Repositories;

namespace E_Wallet.Extensions
{
    public static class ServiceExtension
    {
        public static void AddCustomServices(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();


            var mapperConfig = new MapperConfiguration(mc => {
                mc.AddProfile(new AutoMapperConfig());
                mc.AllowNullCollections = true;
            });

            IMapper mapper = mapperConfig.CreateMapper();

            services.AddSingleton(mapper);
        }
    }
}
