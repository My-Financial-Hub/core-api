namespace FinancialHub.Core.Domain.DTOS.Categories
{
    public class UpdateCategoryDto
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public bool IsActive { get; private set; }

        public UpdateCategoryDto()
        {
            
        }

        public UpdateCategoryDto(string name, string description, bool isActive)
        {
            Name = name;
            Description = description;
            IsActive = isActive;
        }
    }
}
