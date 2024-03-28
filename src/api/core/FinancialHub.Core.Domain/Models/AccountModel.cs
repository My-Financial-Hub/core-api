namespace FinancialHub.Core.Domain.Models
{
    public class AccountModel : BaseModel
    {
        public string Name { get; init; }
        public string Description { get; init; }
        public bool IsActive { get; init; }
    }
}
