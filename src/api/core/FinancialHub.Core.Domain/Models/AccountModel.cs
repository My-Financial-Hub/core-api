using FinancialHub.Common.Models;

namespace FinancialHub.Core.Domain.Models
{
    public class AccountModel : BaseModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
}
