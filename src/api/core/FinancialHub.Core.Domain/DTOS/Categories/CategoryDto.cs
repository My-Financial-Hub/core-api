namespace FinancialHub.Core.Domain.DTOS.Categories
{
    public class CategoryDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public string Description { get; init; }
        public bool IsActive { get; init; }
    }
}
