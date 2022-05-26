using AquaWater.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomanyWaterAPI.BusinessLogic.Services.Intefaces
{
    public interface ITokenGenerator
    {
        public Task<string> GenerateTokenAsync(User user);

    }
}
