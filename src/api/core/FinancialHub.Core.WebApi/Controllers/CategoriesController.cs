using FinancialHub.Core.Domain.DTOS.Categories;
using Microsoft.Extensions.Logging;

namespace FinancialHub.Core.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class CategoriesController : BaseController
    {
        private readonly ICategoriesService service;
        private readonly ILogger<CategoriesController> logger;

        public CategoriesController(ICategoriesService service, ILogger<CategoriesController> logger)
        {
            this.service = service;
            this.logger = logger;
        }

        /// <summary>
        /// Get all categories of the system
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(ListResponse<CategoryDto>), 200)]
        public async Task<IActionResult> GetCategories()
        {
            this.logger.LogInformation("Getting all categories");
            var result = await service.GetAllAsync();

            this.logger.LogInformation("Succesfully returned all categories");
            return ListResponse(result.Data);
        }

        /// <summary>
        /// Creates a category on database
        /// </summary>
        /// <param name="category">Category to be created</param>
        [HttpPost]
        [ProducesResponseType(typeof(SaveResponse<CategoryDto>), 200)]
        [ProducesResponseType(typeof(ValidationsErrorResponse), 400)]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryDto category)
        {
            this.logger.LogInformation("Starting creation of a Category");
            var result = await service.CreateAsync(category);

            if (result.HasError)
            {
                this.logger.LogWarning(
                    "Error creating category : {Message}",
                    result.Error.Message
                );
                return ErrorResponse(result.Error);
            }

            this.logger.LogInformation("Finished creation of a Category");
            return SaveResponse(result.Data);
        }

        /// <summary>
        /// Updates an existing category on database
        /// </summary>
        /// <param name="id">id of the category</param>
        /// <param name="category">category changes</param>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(SaveResponse<CategoryDto>), 200)]
        [ProducesResponseType(typeof(ValidationsErrorResponse), 400)]
        public async Task<IActionResult> UpdateCategory([FromRoute] Guid id, [FromBody] UpdateCategoryDto category)
        {
            this.logger.LogInformation("Starting update of a category");
            var result = await service.UpdateAsync(id, category);

            if (result.HasError)
            {
                this.logger.LogWarning(
                    "Error updating category {id} : {Message}",
                    id, result.Error.Message
                );
                return ErrorResponse(result.Error);
            }

            this.logger.LogInformation("Finished update of a category");
            return SaveResponse(result.Data);
        }

        /// <summary>
        /// Deletes an existing category on database
        /// </summary>
        /// <param name="id">id of the category</param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory([FromRoute] Guid id)
        {
            this.logger.LogInformation("Removing category");
            await service.DeleteAsync(id);
            this.logger.LogInformation("Category removed");

            return NoContent();
        }
    }
}
