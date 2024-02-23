namespace FinancialHub.Core.Domain.Models
{
    public class CategoryModel : BaseModel
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public bool IsActive { get; private set; }
    }
}
