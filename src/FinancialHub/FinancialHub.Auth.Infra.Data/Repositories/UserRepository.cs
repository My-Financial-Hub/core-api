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
            user.Id = null;
            user.CreationTime = DateTimeOffset.Now;
            user.UpdateTime = DateTimeOffset.Now;

            var entry = this.context.Users.Add(user);
            await this.context.SaveChangesAsync();

            return entry.Entity;
        }

        public async Task<UserEntity?> GetAsync(Guid id)
        {
            return await this.context.Users.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<UserEntity> UpdateAsync(UserEntity user)
        {
            user.UpdateTime = DateTimeOffset.Now;

            var res = context.Users.Update(user);
            this.context.Entry(res.Entity).Property(x => x.CreationTime).IsModified = false;

            await context.SaveChangesAsync();

            return res.Entity;
        }
    }
}
