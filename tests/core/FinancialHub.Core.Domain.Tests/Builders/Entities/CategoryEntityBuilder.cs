namespace FinancialHub.Core.Domain.Tests.Builders.Entities
{
    public class CategoryEntityBuilder : BaseEntityBuilder<CategoryEntity>
    {
        public CategoryEntityBuilder() : base()
        {
            this.RuleFor(x => x.Name, fake => fake.Finance.Random.Word());
            this.RuleFor(x => x.Description, fake => fake.Lorem.Sentences(5));
            this.RuleFor(x => x.IsActive, fake => fake.System.Random.Bool());
        }

        public CategoryEntityBuilder WithName(string name)
        {
            this.RuleFor(c => c.Name, name);
            return this;
        }

        public CategoryEntityBuilder WithDescription(string description)
        {
            this.RuleFor(c => c.Description, description);
            return this;
        }

        public CategoryEntityBuilder WithActive(bool isActive)
        {
            this.RuleFor(c => c.IsActive, isActive);
            return this;
        }
    }
}
