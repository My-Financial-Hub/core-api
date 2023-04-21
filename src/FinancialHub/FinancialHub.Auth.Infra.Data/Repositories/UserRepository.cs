using FinancialHub.Auth.Domain.Entities;
using FinancialHub.Auth.Domain.Interfaces.Repositories;
using FinancialHub.Auth.Infra.Data.Contexts;

namespace FinancialHub.Auth.Infra.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly FinancialHubAuthContext context;

        public UserRepository(FinancialHubAuthContext context)
        {
            this.context = context;
        }

        public async Task<UserEntity> CreateAsync(UserEntity user)
        {
            var entry = this.context.Users.Add(user);
            await this.context.SaveChangesAsync();

            return entry.Entity;
        }

        public async Task<UserEntity?> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<UserEntity> UpdateAsync(UserEntity user)
        {
            throw new NotImplementedException();
        }
    }
}
