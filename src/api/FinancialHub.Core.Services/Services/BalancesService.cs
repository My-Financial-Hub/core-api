namespace FinancialHub.Core.Services.Services
{
    public class BalancesService : IBalancesService
    {
        private readonly IMapperWrapper mapper;
        private readonly IBalancesRepository repository;
        private readonly IAccountsRepository accountsRepository;

        public BalancesService(IMapperWrapper mapper, 
            IBalancesRepository repository, IAccountsRepository accountsRepository
            )
        {
            this.mapper = mapper;
            this.repository = repository;
            this.accountsRepository = accountsRepository;
        }

        private async Task<ServiceResult> ValidateAccountAsync(BalanceEntity balance)
        {
            var accountResult = await this.accountsRepository.GetByIdAsync(balance.AccountId);

            if(accountResult == null)
            {
                return new NotFoundError($"Not found Account with id {balance.AccountId}");
            }

            return new ServiceResult();
        }

        public async Task<ServiceResult<BalanceModel>> CreateAsync(BalanceModel balance)
        {
            var entity = this.mapper.Map<BalanceEntity>(balance);
            entity.Amount = 0;

            var validationResult = await this.ValidateAccountAsync(entity);
            if (validationResult.HasError)
            {
                return validationResult.Error;
            }

            entity = await this.repository.CreateAsync(entity);

            return mapper.Map<BalanceModel>(entity);
        }

        public async Task<ServiceResult<int>> DeleteAsync(Guid id)
        {
            var count = await this.repository.DeleteAsync(id);

            return new ServiceResult<int>(count);
        }

        public async Task<ServiceResult<BalanceModel>> GetByIdAsync(Guid id)
        {
            var entity = await this.repository.GetByIdAsync(id);

            if(entity == null)
            {
                return new NotFoundError($"Not found Balance with id {id}");
            }

            return this.mapper.Map<BalanceModel>(entity);
        }

        public async Task<ServiceResult<ICollection<BalanceModel>>> GetAllByAccountAsync(Guid accountId)
        {
            var entities = await this.repository.GetAsync(x => x.AccountId == accountId);

            return this.mapper.Map<ICollection<BalanceModel>>(entities).ToArray();
        }

        public async Task<ServiceResult<BalanceModel>> UpdateAsync(Guid id, BalanceModel balance)
        {
            var entityResult = await this.GetByIdAsync(id);
            if (entityResult.HasError)
            {
                return entityResult.Error;
            }

            var entity = this.mapper.Map<BalanceEntity>(balance);
            entity.Id = id;

            var validationResult = await this.ValidateAccountAsync(entity);
            if (validationResult.HasError)
            {
                return validationResult.Error;
            }

            entity = await this.repository.UpdateAsync(entity);

            return mapper.Map<BalanceModel>(entity);
        }

        public async Task<ServiceResult<BalanceModel>> UpdateAmountAsync(Guid id, decimal newAmount)
        {
            var balanceResult = await this.GetByIdAsync(id);
            if (balanceResult.HasError)
            {
                return balanceResult.Error;
            }

            var newBalance = await repository.ChangeAmountAsync(id, newAmount);

            return mapper.Map<BalanceModel>(newBalance);
        }
    }
}
