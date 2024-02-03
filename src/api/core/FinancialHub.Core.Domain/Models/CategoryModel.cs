namespace FinancialHub.Core.Domain.Models
{
    public class CategoryModel : BaseModel
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public bool IsActive { get; private set; }

        public CategoryModel(Guid? id,string name, string description, bool isActive) : base(id)
        {
            Name = name;
            Description = description;
            IsActive = isActive;
        }
    }
}
