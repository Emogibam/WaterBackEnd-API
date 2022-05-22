using AquaWater.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using RomanyWaterAPI.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomanyWaterAPI.BusinessLogic.ConfigurationExtentions
{
    public static class IdentityConfiguration
    {
        public static void ConfigurationIdentity(this IServiceCollection services)
        {
            services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequiredUniqueChars = 1;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequiredLength = 7;
                options.SignIn.RequireConfirmedEmail = true;
            })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();
        }
    }
}
