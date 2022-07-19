using AquaWater.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using RomanyWaterAPI.BusinessLogic.Services.Intefaces;
using RomanyWaterAPI.Data.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomanyWaterAPI.BusinessLogic.Services.Implementations
{
    public class FindApplicationUser : IFindApplicationUser
    {
        private readonly IGenericRepository<Customer> _customerGenericRepo;
        private readonly IGenericRepository<AdminUser> _adminGenericRepo;
        private readonly IGenericRepository<CompanyManager> _companyMangerGenericRepo;

        public FindApplicationUser(IGenericRepository<Customer> customerGenericRepo, IGenericRepository<AdminUser> adminGenericRepo,
            IGenericRepository<CompanyManager> companyMangerGenericRepo)
        {
            _customerGenericRepo = customerGenericRepo;
            _adminGenericRepo = adminGenericRepo;
            _companyMangerGenericRepo = companyMangerGenericRepo;
        }


        public async Task<Customer> GetCustomerByUserIdAsync(string id)
        {
            var customer = await _customerGenericRepo.TableNoTracking
                            .Where(x => x.UserId == id)
                            .Include(x => x.User).FirstOrDefaultAsync();
            return customer;
        }

        public async Task<AdminUser> GetAdminByUserIdAsync(string id)
        {
            var adminUser = await _adminGenericRepo.TableNoTracking
                                .Include(x => x.User).FirstOrDefaultAsync(x => x.UserId == id);
            return adminUser;
        }

        public async Task<CompanyManager> GetCompanyManagerByUserIdAsync(string id)
        {
            var companyManager = await _companyMangerGenericRepo.TableNoTracking
                                    .Include(x => x.User).FirstOrDefaultAsync(x => x.UserId == id);
            return companyManager;
        }
    }
}
