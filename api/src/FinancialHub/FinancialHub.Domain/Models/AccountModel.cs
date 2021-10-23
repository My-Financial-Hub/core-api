using FinancialHub.Domain.Model;

namespace FinancialHub.Domain.Models
{
    public class AccountModel : BaseModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Currency { get; set; }
        public bool IsActive { get; set; }
    }
}
