namespace FinancialHub.Core.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class CategoriesController : Controller
    {
        private readonly ICategoriesService service;

        public CategoriesController(ICategoriesService service)
        {
            this.service = service;
        }

        /// <summary>
        /// Get all categorys of the system (will be changed to only one user)
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(ListResponse<CategoryModel>), 200)]
        public async Task<IActionResult> GetMyCategories()
        {
            var result = await service.GetAllByUserAsync("mock");

            return Ok(new ListResponse<CategoryModel>(result.Data));
        }

        /// <summary>
        /// Creates an category on database (will be changed to only one user)
        /// </summary>
        /// <param name="category">Account to be created</param>
        [HttpPost]
        [ProducesResponseType(typeof(SaveResponse<CategoryModel>), 200)]
        [ProducesResponseType(typeof(ValidationErrorResponse), 400)]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryModel category)
        {
            var result = await service.CreateAsync(category);

            if (result.HasError)
            {
                return StatusCode(
                    result.Error.Code,
                    new ValidationErrorResponse(result.Error.Message)
                 );
            }

            return Ok(new SaveResponse<CategoryModel>(result.Data));
        }

        /// <summary>
        /// Updates an existing category on database
        /// </summary>
        /// <param name="id">id of the category</param>
        /// <param name="category">category changes</param>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(SaveResponse<CategoryModel>), 200)]
        [ProducesResponseType(typeof(ValidationErrorResponse), 400)]
        public async Task<IActionResult> UpdateCategory([FromRoute] Guid id, [FromBody] CategoryModel category)
        {
            var result = await service.UpdateAsync(id, category);

            if (result.HasError)
            {
                return StatusCode(
                    result.Error.Code,
                    new ValidationErrorResponse(result.Error.Message)
                 );
            }

            return Ok(new SaveResponse<CategoryModel>(result.Data));
        }

        /// <summary>
        /// Deletes an existing category on database
        /// </summary>
        /// <param name="id">id of the category</param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory([FromRoute] Guid id)
        {
            await service.DeleteAsync(id);
            return NoContent();
        }
    }
}
