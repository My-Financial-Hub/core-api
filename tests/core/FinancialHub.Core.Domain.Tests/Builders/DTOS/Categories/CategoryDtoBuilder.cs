using Bogus;
using FinancialHub.Core.Domain.DTOS.Categories;

namespace FinancialHub.Core.Domain.Tests.Builders.DTOS.Categories
{
    public class CategoryDtoBuilder : Faker<CategoryDto>
    {
        public CategoryDtoBuilder() : base()
        {
            this.RuleFor(x => x.Id, fake => fake.Random.Uuid());
            this.RuleFor(x => x.Name, fake => fake.Finance.Random.Word());
            this.RuleFor(x => x.Description, fake => fake.Lorem.Sentences(5));
            this.RuleFor(x => x.IsActive, fake => fake.System.Random.Bool());
        }

        public CategoryDtoBuilder WithName(string name)
        {
            this.RuleFor(c => c.Name, name);
            return this;
        }

        public CategoryDtoBuilder WithDescription(string description)
        {
            this.RuleFor(c => c.Description, description);
            return this;
        }

        public CategoryDtoBuilder WithActive(bool isActive)
        {
            this.RuleFor(c => c.IsActive, isActive);
            return this;
        }

        public CategoryDtoBuilder FromCreateDto(CreateCategoryDto category)
        {
            this
                .WithName(category.Name)
                .WithDescription(category.Description)
                .WithActive(category.IsActive);
            return this;
        }

        public CategoryDtoBuilder FromUpdateDto(UpdateCategoryDto category)
        {
            this
                .WithName(category.Name)
                .WithDescription(category.Description)
                .WithActive(category.IsActive);
            return this;
        }
    }
}
