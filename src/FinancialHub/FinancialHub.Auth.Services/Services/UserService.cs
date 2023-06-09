using AutoMapper;
using FinancialHub.Domain.Results;
using FinancialHub.Auth.Domain.Interfaces.Services;
using FinancialHub.Auth.Domain.Models;
using FinancialHub.Auth.Domain.Interfaces.Providers;
using FinancialHub.Domain.Results.Errors;

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
            return await provider.CreateAsync(user);
        }

        public async Task<ServiceResult<UserModel>> GetAsync(Guid id)
        {
            var user = await provider.GetAsync(id);

            if(user == null)
            {
                return new ServiceError(404, "User not found");
        }

            return user;
        }

        public Task<ServiceResult<UserModel>> GetAsync(LoginModel user)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResult<UserModel>> UpdateAsync(Guid id,UserModel user)
        {
            var getByIdResult = await GetAsync(id);
            if (getByIdResult.HasError)
            {
                return getByIdResult;
            }

            user.Id = id;
            var updatedUser = await provider.UpdateAsync(user);

            return updatedUser;
        }
    }
}
