namespace FinancialHub.Core.Application.Services
{
    public class CategoriesService : ICategoriesService
    {
        private readonly IMapperWrapper mapper;
        private readonly ICategoriesRepository repository;

        public CategoriesService(IMapperWrapper mapper, ICategoriesRepository repository)
        {
            this.mapper = mapper;
            this.repository = repository;
        }

        public async Task<ServiceResult<CategoryModel>> CreateAsync(CategoryModel category)
        {
            var entity = mapper.Map<CategoryEntity>(category);

            entity = await this.repository.CreateAsync(entity);

            return mapper.Map<CategoryModel>(entity);
        }

        public async Task<ServiceResult<int>> DeleteAsync(Guid id)
        {
            return await this.repository.DeleteAsync(id);
        }

        public async Task<ServiceResult<ICollection<CategoryModel>>> GetAllByUserAsync(string userId)
        {
            var entities = await this.repository.GetAllAsync();

            var list = this.mapper.Map<ICollection<CategoryModel>>(entities);

            return list.ToArray();
        }

        public async Task<ServiceResult<CategoryModel>> UpdateAsync(Guid id, CategoryModel category)
        {
            var entity = await this.repository.GetByIdAsync(id);

            if (entity == null)
            {
                return new NotFoundError($"Not found category with id {id}");
            }
            entity.Id = id;

            entity = this.mapper.Map<CategoryEntity>(category);
            entity = await this.repository.UpdateAsync(entity);

            return mapper.Map<CategoryModel>(entity);
        }
    }
}
