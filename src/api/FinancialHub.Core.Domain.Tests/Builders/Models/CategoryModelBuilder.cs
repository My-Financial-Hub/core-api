namespace FinancialHub.Core.Domain.Tests.Builders.Models
{
    public class CategoryModelBuilder : BaseModelBuilder<CategoryModel>
    {
        public CategoryModelBuilder() : base()
        {
            this.RuleFor(x => x.Name, fake => fake.Finance.Random.Word());
            this.RuleFor(x => x.Description, fake => fake.Lorem.Sentences(5));
            this.RuleFor(x => x.IsActive, fake => fake.System.Random.Bool());;
        }

        public CategoryModelBuilder WithName(string name)
        {
            this.RuleFor(c => c.Name, name);
            return this;
        }

        public CategoryModelBuilder WithDescription(string description)
        {
            this.RuleFor(c => c.Description, description);
            return this;
        }

        public CategoryModelBuilder WithActive(bool isActive)
        {
            this.RuleFor(c => c.IsActive, isActive);
            return this;
        }
    }
}
