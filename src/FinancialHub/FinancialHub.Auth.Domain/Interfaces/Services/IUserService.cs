using FinancialHub.Auth.Domain.Models;

namespace FinancialHub.Auth.Domain.Interfaces.Services
{
    public interface IUserService
    {
        Task<ServiceResult<UserModel>> GetAsync(Guid id);
        Task<ServiceResult<UserModel>> GetAsync(LoginModel user);
        Task<ServiceResult<UserModel>> CreateAsync(UserModel user);
        Task<ServiceResult<UserModel>> UpdateAsync(Guid id, UserModel user);
    }
}
