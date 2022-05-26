using AquaWater.Domain.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using RomanyWaterAPI.BusinessLogic.Services.Intefaces;
using RomanyWaterAPI.DTO.Request;
using RomanyWaterAPI.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomanyWaterAPI.BusinessLogic.Services.Implementations
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<User> _userManager;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly IMapper _mapper;
        private readonly IConfirmationMailService _confirmationMailService;

        public AuthenticationService(UserManager<User> userManager, ITokenGenerator tokenGenerator,
            IMapper mapper, IConfirmationMailService confirmationMailService)
        {
            _userManager = userManager;
            _tokenGenerator = tokenGenerator;
            _mapper = mapper;
            _confirmationMailService = confirmationMailService;
        }

        public async Task<Response<UserResponseDto>> LoginAsync(UserRequestDTO userRequest)
        {
            var user = await _userManager.FindByEmailAsync(userRequest.Email);
            if (user != null)
            {
                if (await _userManager.CheckPasswordAsync(user, userRequest.Password))
                {
                    if (user.EmailConfirmed)
                    {
                        return new Response<UserResponseDto>()
                        {
                            Message = "Login Successful",
                            Success = true,
                            Data = new UserResponseDto
                            {
                                Email = user.Email,
                                FullName = $"{user.FirstName + " " + user.LastName}",
                                Id = user.Id,
                                Token = await _tokenGenerator.GenerateTokenAsync(user)
                            }
                        };
                    }
                    return new Response<UserResponseDto>();

                }
                throw new AccessViolationException("Kindly verify your email address to login");
            }
            throw new AccessViolationException("Invalid credentials");
        }
    }
}

    

