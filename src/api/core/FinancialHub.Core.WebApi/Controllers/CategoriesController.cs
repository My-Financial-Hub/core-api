﻿using FinancialHub.Core.Domain.DTOS.Categories;

namespace FinancialHub.Core.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class CategoriesController : BaseController
    {
        private readonly ICategoriesService service;

        public CategoriesController(ICategoriesService service)
        {
            this.service = service;
        }

        /// <summary>
        /// Get all categorys of the system
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(ListResponse<CategoryDto>), 200)]
        public async Task<IActionResult> GetMyCategories()
        {
            var result = await service.GetAllAsync();

            return Ok(new ListResponse<CategoryDto>(result.Data));
        }

        /// <summary>
        /// Creates an category on database
        /// </summary>
        /// <param name="category">Account to be created</param>
        [HttpPost]
        [ProducesResponseType(typeof(SaveResponse<CategoryDto>), 200)]
        [ProducesResponseType(typeof(ValidationsErrorResponse), 400)]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryDto category)
        {
            var result = await service.CreateAsync(category);

            if (result.HasError)
            {
                return ErrorResponse(result.Error);
            }

            return Ok(new SaveResponse<CategoryDto>(result.Data));
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
            var result = await service.UpdateAsync(id, category);

            if (result.HasError)
            {
                return ErrorResponse(result.Error);
            }

            return Ok(new SaveResponse<CategoryDto>(result.Data));
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
