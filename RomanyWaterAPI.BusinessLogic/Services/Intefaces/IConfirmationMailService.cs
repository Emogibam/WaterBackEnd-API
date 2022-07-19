using RomanyWaterAPI.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomanyWaterAPI.BusinessLogic.Services.Intefaces
{
    public interface IConfirmationMailService
    {
        Task SendAConfirmationEmail(UserResponseDto user);
        Task SendAConfirmationEmailForResetPassword(UserResponseDto user);
        Task SendConfirmTokenEmail(string userId);
        Task<Response<string>> SendReminderEmail(string companyManagerId, string customerId);
    }
}
