using RomanyWaterAPI.DTO.Request;
using RomanyWaterAPI.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomanyWaterAPI.BusinessLogic.Services.Intefaces
{
    public interface IAuthenticationServices
    {
        //Task<Response<string>> EmailConfirmationAsync(ConfirmEmailRequestDTO confirmEmailRequest);
        Task<Response<UserResponseDto>> LoginAsync(UserRequestDTO userRequest);
        //Task<Response<string>> UpdatePassword(UpdatePasswordDTO updatePasswordDTO);
        //Task<Response<string>> ForgotPasswordAsync(string email);
        //Task<Response<string>> ResetPasswordAsync(ResetPasswordDTO resetPassword);
    }
}
