namespace FinancialHub.Domain.Tests.Builders.Entities
{
    public class AccountEntityBuilder : BaseEntityBuilder<AccountEntity>
    {
        public AccountEntityBuilder() : base()
        {
            this.RuleFor(x => x.Name, fake => fake.Finance.AccountName());
            this.RuleFor(x => x.Description, fake => fake.Lorem.Sentences(5));
            this.RuleFor(x => x.IsActive, fake => fake.System.Random.Bool());
        }

        public AccountEntityBuilder WithName(string name)
        {
            this.RuleFor(c => c.Name, name);
            return this;
        }

        public AccountEntityBuilder WithDescription(string description)
        {
            this.RuleFor(c => c.Description, description);
            return this;
        }

        public AccountEntityBuilder WithActive(bool isActive)
        {
            this.RuleFor(c => c.IsActive, isActive);
            return this;
        }
    }
}
