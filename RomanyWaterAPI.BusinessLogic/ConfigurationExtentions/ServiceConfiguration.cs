using Microsoft.Extensions.DependencyInjection;
using RomanyWaterAPI.Data.Repository.Implimentation;
using RomanyWaterAPI.Data.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomanyWaterAPI.BusinessLogic.ConfigurationExtentions
{
    public static class ServiceConfiguration
    {
        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

        }

    }
}
