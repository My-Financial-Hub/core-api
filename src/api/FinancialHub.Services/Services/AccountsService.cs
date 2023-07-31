namespace FinancialHub.Services.Services
{
    public class AccountsService : IAccountsService
    {
        private readonly IMapperWrapper mapper;
        private readonly IAccountsRepository repository;

        public AccountsService(IMapperWrapper mapper,IAccountsRepository repository)
        {
            this.mapper = mapper;
            this.repository = repository;
        }

        public async Task<ServiceResult<AccountModel>> CreateAsync(AccountModel account)
        {
            var entity = mapper.Map<AccountEntity>(account);

            entity = await this.repository.CreateAsync(entity);

            return mapper.Map<AccountModel>(entity);
        }

        public async Task<ServiceResult<int>> DeleteAsync(Guid id)
        {
            var count = await this.repository.DeleteAsync(id);

            return new ServiceResult<int>(count);
        }

        public async Task<ServiceResult<ICollection<AccountModel>>> GetAllByUserAsync(string userId)
        {
            var entities = await this.repository.GetAllAsync();

            var list = this.mapper.Map<ICollection<AccountModel>>(entities);

            return list.ToArray();
        }

        public async Task<ServiceResult<AccountModel>> GetByIdAsync(Guid id)
        {
            var entity = await this.repository.GetByIdAsync(id);

            return this.mapper.Map<AccountModel>(entity);
        }

        public async Task<ServiceResult<AccountModel>> UpdateAsync(Guid id, AccountModel account)
        {
            var entity = await this.repository.GetByIdAsync(id);

            if (entity == null)
            {
                return new NotFoundError($"Not found account with id {id}");
            }

            entity = this.mapper.Map<AccountEntity>(account);

            entity = await this.repository.UpdateAsync(entity);

            return mapper.Map<AccountModel>(entity);
        }
    }
}
