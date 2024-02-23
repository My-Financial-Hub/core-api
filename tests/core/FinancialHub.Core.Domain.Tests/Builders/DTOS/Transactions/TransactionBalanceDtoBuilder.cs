using Bogus;
using FinancialHub.Core.Domain.DTOS.Transactions;

namespace FinancialHub.Core.Domain.Tests.Builders.DTOS.Transactions
{
    public class TransactionBalanceDtoBuilder : Faker<TransactionBalanceDto>
    {
        public TransactionBalanceDtoBuilder() : base()
        {
            var account = new TransactionAccountDtoBuilder().Generate();

            this.RuleFor(x => x.Id, fake => fake.Random.Uuid());
            this.RuleFor(x => x.Name, fake => fake.Finance.AccountName());
            this.RuleFor(x => x.Currency, fake => fake.Finance.Currency().Code);
            this.RuleFor(x => x.Account, fake => account);
        }

        public TransactionBalanceDtoBuilder WithId(Guid guid)
        {
            this.RuleFor(x => x.Id, guid);
            return this;
        }

        public TransactionBalanceDtoBuilder WithName(string name)
        {
            this.RuleFor(c => c.Name, name);
            return this;
        }

        public TransactionBalanceDtoBuilder WithCurrency(string currency)
        {
            this.RuleFor(c => c.Currency, currency);
            return this;
        }

        public TransactionBalanceDtoBuilder WithAccount(TransactionAccountDto account)
        {
            this.RuleFor(x => x.Account, fake => account);
            return this;
        }
    }
}
