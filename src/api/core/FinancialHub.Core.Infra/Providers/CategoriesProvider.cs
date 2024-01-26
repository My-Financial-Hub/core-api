namespace FinancialHub.Core.Infra.Providers
{
    public class CategoriesProvider : ICategoriesProvider
    {
        private readonly IMapper mapper;
        private readonly ICategoriesRepository repository;

        public CategoriesProvider(IMapper mapper, ICategoriesRepository repository)
        {
            this.mapper = mapper;
            this.repository = repository;
        }

        public async Task<CategoryModel> CreateAsync(CategoryModel category)
        {
            var categoryEntity = mapper.Map<CategoryEntity>(category);

            var createdAccount = await this.repository.CreateAsync(categoryEntity);
            await this.repository.CommitAsync();

            return mapper.Map<CategoryModel>(createdAccount);
        }

        public async Task<int> DeleteAsync(Guid id)
        {
            await repository.DeleteAsync(id);

            return await this.repository.CommitAsync();
        }

        public async Task<ICollection<CategoryModel>> GetAllAsync()
        {
            var categories = await this.repository.GetAllAsync();

            return mapper.Map<ICollection<CategoryModel>>(categories);
        }

        public async Task<CategoryModel?> GetByIdAsync(Guid id)
        {
            var category = await this.repository.GetByIdAsync(id);
            if(category == null)
            {
                return null;
            }

            return mapper.Map<CategoryModel>(category);
        }

        public async Task<CategoryModel?> UpdateAsync(Guid id, CategoryModel category)
        {
            var categoryEntity = mapper.Map<CategoryEntity>(category);
            categoryEntity.Id = id;

            var updatedCategory = await this.repository.UpdateAsync(categoryEntity);
            await this.repository.CommitAsync();

            return mapper.Map<CategoryModel>(updatedCategory);
        }
    }
}
