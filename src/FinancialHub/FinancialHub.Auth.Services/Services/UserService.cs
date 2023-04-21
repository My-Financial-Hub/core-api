using FinancialHub.Domain.Results;
using FinancialHub.Auth.Domain.Interfaces.Services;
using FinancialHub.Auth.Domain.Models;
using FinancialHub.Auth.Domain.Interfaces.Providers;

namespace FinancialHub.Auth.Services.Services
{
    public class UserService : IUserService
    {
        private readonly IUserProvider provider;

        public UserService(IUserProvider provider)
        {
            this.provider = provider;
        }

        public async Task<ServiceResult<UserModel>> CreateAsync(UserModel user)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResult<UserModel>> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResult<UserModel>> LoginAsync(LoginModel user)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResult<UserModel>> UpdateAsync(UserModel user)
        {
            throw new NotImplementedException();
        }
    }
}
