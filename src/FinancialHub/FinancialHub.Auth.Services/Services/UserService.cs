using AutoMapper;
using FinancialHub.Domain.Results;
using FinancialHub.Auth.Domain.Interfaces.Repositories;
using FinancialHub.Auth.Domain.Interfaces.Services;
using FinancialHub.Auth.Domain.Models;

namespace FinancialHub.Auth.Services.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository repository;
        private readonly IMapper mapper;

        public UserService(IUserRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<ServiceResult<UserModel>> CreateAsync(UserModel user)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResult<ICollection<UserModel>>> GetAllAsync()
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
