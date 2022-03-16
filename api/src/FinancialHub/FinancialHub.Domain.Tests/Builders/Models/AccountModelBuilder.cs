using FinancialHub.Domain.Models;

namespace FinancialHub.Domain.Tests.Builders.Models
{
    public class AccountModelBuilder : BaseModelBuilder<AccountModel>
    {
        public AccountModelBuilder() : base()
        {
            this.RuleFor(x => x.Name, fake => fake.Finance.AccountName());
            this.RuleFor(x => x.Currency, fake => fake.Finance.Currency().Code);
            this.RuleFor(x => x.Description, fake => fake.Lorem.Sentences(5));
            this.RuleFor(x => x.IsActive, fake => fake.System.Random.Bool());
        }

        public AccountModelBuilder WithName(string name)
        {
            this.RuleFor(c => c.Name, name);
            return this;
        }

        public AccountModelBuilder WithCurrency(string currency)
        {
            this.RuleFor(c => c.Currency, currency);
            return this;
        }

        public AccountModelBuilder WithDescription(string description)
        {
            this.RuleFor(c => c.Description, description);
            return this;
        }

        public AccountModelBuilder WithActive(bool isActive)
        {
            this.RuleFor(c => c.IsActive, isActive);
            return this;
        }
    }
}
