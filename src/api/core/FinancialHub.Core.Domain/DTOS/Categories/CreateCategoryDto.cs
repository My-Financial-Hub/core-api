namespace FinancialHub.Core.Domain.DTOS.Categories
{
    public class CreateCategoryDto
    {
        public string Name { get; init; }
        public string Description { get; init; }
        public bool IsActive { get; init; }
    }
}
