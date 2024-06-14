using FinancialHub.Common.Models;

namespace FinancialHub.Core.Domain.Balances.Models
{
    public class Category : BaseModel
    {
        public string Name { get; init; }
        public string Description { get; init; }
        public bool IsActive { get; init; }
    }
}
