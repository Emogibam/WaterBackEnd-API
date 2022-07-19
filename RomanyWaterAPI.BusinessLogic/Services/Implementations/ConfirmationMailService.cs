﻿using AquaWater.Domain.Entities;
using MailKit;
using Microsoft.Extensions.Configuration;
using RomanyWaterAPI.BusinessLogic.Services.Intefaces;
using RomanyWaterAPI.Data.Repository.Interface;
using RomanyWaterAPI.DTO.Response;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomanyWaterAPI.BusinessLogic.Services.Implementations
{
    public class ConfirmationMailService : IConfirmationMailService
    {
        private readonly IMailService _mailService;
        private readonly IConfiguration _configuration;
        private readonly IGenericRepository<Order> _orderRepository;
        private readonly IGenericRepository<Customer> _customerRepository;
        private readonly IGenericRepository<User> _userRepository;
        private readonly IGenericRepository<CompanyManager> _companyManagerRepository;
        private readonly IFindApplicationUser _findApplicationUser;
        public ConfirmationMailService(IMailService mailService, IConfiguration configuration, IGenericRepository<Order> orderRepository, IGenericRepository<User> userRepository, 
            IGenericRepository<Customer> customerRepository, IGenericRepository<CompanyManager> companyManagerRepository, IFindApplicationUser findApplicationUser)
        {
            _mailService = mailService;
            _configuration = configuration;
            _orderRepository = orderRepository;
            _customerRepository = customerRepository;
            _userRepository = userRepository;
            _companyManagerRepository = companyManagerRepository;
            _findApplicationUser = findApplicationUser;
        }

        public async Task SendAConfirmationEmail(UserResponseDto user)
        {
            var template = _mailService.GetEmailTemplate("EmailTemplate.html");
            TextInfo textInfo = new CultureInfo("en-GB", false).TextInfo;
            var userName = textInfo.ToTitleCase(user.FullName);

            var encodedToken = TokenConverter.EncodeToken(user.Token);
            var link = $"{_configuration["Application:AppDomain"]}/Authentication/ConfirmEmail?email={user.Email}/token={encodedToken}";

            template = template.Replace("{User}", $"{userName}");
            template = template.Replace("{Body}", "Welcome to AquaWater Plc, Registration was successful, click the link below");
            template = template.Replace("{Linkl}", link);
            template = template.Replace("{Details}", $"If you have trouble clicking on the link above you can paste this link on your browser {link}");
            template = template.Replace("{Action}", "Confirm Email");

            var mailRequest = new MailRequest
            {
                ToEmail = user.Email,
                Body = template,
                Subject = "Confirm Email"
            };

            await _mailService.SendEmailAsync(mailRequest);
        }

        public async Task SendAConfirmationEmailForResetPassword(UserResponseDto user)
        {
            var template = _mailService.GetEmailTemplate("EmailTemplate.html");
            TextInfo textInfo = new CultureInfo("en-GB", false).TextInfo;

            var userName = textInfo.ToTitleCase(user.FullName);
            var encodedToken = TokenConverter.EncodeToken(user.Token);
            var link = $"{_configuration["Application:AppDomain"]}/Authentication/ResetPassword?email={user.Email}/token={encodedToken}";

            string message = "Reset Password";

            template = template.Replace("{User}", $"{userName}");
            template = template.Replace("{Body}", "Welcome to AquaWater Plc,To reset password, click the link below");
            template = template.Replace("{Link}", link);
            template = template.Replace("{Details}", $"If you have trouble clicking on the link above you can paste this link on your browser {link}");
            template = template.Replace("{Action}", $"{message}");

            var mailRequest = new MailRequest
            {
                ToEmail = user.Email,
                Body = template,
                Subject = "Reset Password"
            };

            await _mailService.SendEmailAsync(mailRequest);
        }
        public async Task SendConfirmTokenEmail(string userId)
        {
            Customer customer = _customerRepository.Table.FirstOrDefault(x => x.UserId == userId);
            User user = _userRepository.Table.FirstOrDefault(x => x.Id == userId);
            var order = _orderRepository.Table.FirstOrDefault(x => x.CustomerId == customer.Id);
            var template = _mailService.GetEmailTemplate("EmailTemplate.html");
            TextInfo textInfo = new CultureInfo("en-GB", false).TextInfo;
            var userName = textInfo.ToTitleCase(user.FirstName + " " + user.LastName);
            template = template.Replace("{User}", $"{userName}");
            template = template.Replace("{Body}", "Payment was successful, this is your token: " + order.OTP);
            template = template.Replace("{Details}", "");
            template = template.Replace("{Action}", $"{_configuration["Application:AppDomain"]}");
            var mailRequest = new MailRequest
            {
                ToEmail = user.Email,
                Body = template,
                Subject = "Payment Confirmation Token"
            };

            await _mailService.SendEmailAsync(mailRequest);
        }

        public async Task<Response<string>> SendReminderEmail(string companyManagerId, string customerId)
        {
            Customer customer = _customerRepository.Table.FirstOrDefault(x => x.Id == Guid.Parse(customerId));
            CompanyManager companyManager = await _findApplicationUser.GetCompanyManagerByUserIdAsync(companyManagerId);
            var template = _mailService.GetEmailTemplate("ReminderEmailTemplate.html");
            TextInfo textInfo = new CultureInfo("en-GB", false).TextInfo;
            var userName = textInfo.ToTitleCase(customer.User.FirstName);
            template = template.Replace("{User}", $"{userName}");
            template = template.Replace("{Body}", "Welcome to AquaWater Plc,This is to remind you of your order");
            template = template.Replace("{Details}", " This will be gotten from the andriod guys");
            var mailRequest = new MailRequest
            {
                ToEmail = customer.User.Email,
                Body = template,
                Subject = "Reminder!"
            };

            await _mailService.SendEmailAsync(mailRequest);
            return new Response<string>()
            {
                Message = $"Your email is successfully sent",
                Success = true
            };
        }
    }
}

