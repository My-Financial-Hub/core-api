using FinancialHub.Common.Results;
using FinancialHub.Auth.Domain.Models;

namespace FinancialHub.Auth.Domain.Interfaces.Services
{
    public interface ISignupService
    {
        Task<ServiceResult<UserModel>> CreateAccountAsync(SignupModel signup);
    }
}
