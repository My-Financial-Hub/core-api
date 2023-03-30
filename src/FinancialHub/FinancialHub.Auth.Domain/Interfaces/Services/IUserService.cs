using FinancialHub.Auth.Domain.Models;
using FinancialHub.Domain.Results;

namespace FinancialHub.Auth.Domain.Interfaces.Services
{
    public interface IUserService
    {
        Task<ServiceResult<ICollection<UserModel>>> GetAllAsync();
        Task<ServiceResult<UserModel>> GetAsync(Guid id);
        Task<ServiceResult<UserModel>> LoginAsync(LoginModel user);
        Task<ServiceResult<UserModel>> CreateAsync(UserModel user);
        Task<ServiceResult<UserModel>> UpdateAsync(UserModel user);
    }
}
