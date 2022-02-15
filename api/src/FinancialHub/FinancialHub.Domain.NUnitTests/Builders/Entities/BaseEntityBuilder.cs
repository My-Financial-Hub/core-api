using Bogus;
using System;
using FinancialHub.Domain.Entities;

namespace FinancialHub.Domain.Tests.Builders.Entities
{
    public abstract class BaseEntityBuilder<T> : Faker<T>
        where T : BaseEntity
    {
        public BaseEntityBuilder()
        {
            this.RuleFor(x => x.CreationTime, fake => DateTimeOffset.Now);
            this.RuleFor(x => x.UpdateTime, fake => DateTimeOffset.Now);
        }

        public BaseEntityBuilder<T> WithId(Guid id)
        {
            this.RuleFor(c => c.Id, id);
            return this;
        }

        public BaseEntityBuilder<T> WithCreationTime(DateTimeOffset creationTime)
        {
            this.RuleFor(c => c.CreationTime, creationTime);
            return this;
        }

        public BaseEntityBuilder<T> WithUpdateTime(DateTimeOffset updateTime)
        {
            this.RuleFor(c => c.UpdateTime, updateTime);
            return this;
        }
    }
}
