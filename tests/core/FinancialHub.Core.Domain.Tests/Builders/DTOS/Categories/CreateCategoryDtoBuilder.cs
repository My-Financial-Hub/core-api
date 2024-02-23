﻿using Bogus;
using FinancialHub.Core.Domain.DTOS.Categories;

namespace FinancialHub.Core.Domain.Tests.Builders.DTOS.Categories
{
    public class CreateCategoryDtoBuilder : Faker<CreateCategoryDto>
    {
        public CreateCategoryDtoBuilder() : base()
        {
            this.RuleFor(x => x.Name, fake => fake.Finance.Random.Word());
            this.RuleFor(x => x.Description, fake => fake.Lorem.Sentences(5));
            this.RuleFor(x => x.IsActive, fake => fake.System.Random.Bool());
        }

        public CreateCategoryDtoBuilder WithName(string name)
        {
            this.RuleFor(c => c.Name, name);
            return this;
        }

        public CreateCategoryDtoBuilder WithDescription(string description)
        {
            this.RuleFor(c => c.Description, description);
            return this;
        }

        public CreateCategoryDtoBuilder WithActive(bool isActive)
        {
            this.RuleFor(c => c.IsActive, isActive);
            return this;
        }
    }
}
