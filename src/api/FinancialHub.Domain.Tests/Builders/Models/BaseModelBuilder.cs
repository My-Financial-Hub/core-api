using Bogus;
using FinancialHub.Domain.Model;
using System;

namespace FinancialHub.Domain.Tests.Builders.Models
{
    public class BaseModelBuilder<T> : Faker<T> 
        where T : BaseModel
    {
        public BaseModelBuilder()
        {
            this.RuleFor(x => x.Id,y => y.System.Random.Guid());
        }

        public BaseModelBuilder<T> WithId(Guid guid)
        {
            this.RuleFor(x => x.Id, guid);
            return this;
        }
    }
}
