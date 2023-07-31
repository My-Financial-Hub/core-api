using Bogus;
using FinancialHub.Common.Models;

namespace FinancialHub.Domains.Tests.Builders.Models
{
    public abstract class BaseModelBuilder<T> : Faker<T>
        where T : BaseModel
    {
        protected BaseModelBuilder()
        {
            this.RuleFor(x => x.Id, y => y.System.Random.Guid());
        }

        public BaseModelBuilder<T> WithId(Guid guid)
        {
            this.RuleFor(x => x.Id, guid);
            return this;
        }
    }
}
