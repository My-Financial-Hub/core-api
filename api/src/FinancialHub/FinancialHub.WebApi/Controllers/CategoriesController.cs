using FinancialHub.Domain.Interfaces.Services;
using FinancialHub.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinancialHub.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    [ProducesErrorResponseType(typeof(Exception))]
    public class CategoriesController : Controller
    {
        private readonly ICategoriesService service;

        public CategoriesController(ICategoriesService service)
        {
            this.service = service;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ICollection<CategoryModel>), 200)]
        /// <summary>
        /// Get all categorys of the system (will be changed to only one user)
        /// </summary>
        public async Task<IActionResult> GetMyCategories()
        {
            var response = await service.GetAllByUserAsync("mock");
            return Ok(response.Data);
        }

        [HttpPost]
        [ProducesResponseType(typeof(CategoryModel), 200)]
        /// <summary>
        /// Creates an category on database (will be changed to only one user)
        /// </summary>
        /// <param name="category">Account to be created</param>
        public async Task<IActionResult> CreateCategory([FromBody] CategoryModel category)
        {
            var response = await service.CreateAsync(category);
            return Ok(response.Data);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(CategoryModel), 200)]
        /// <summary>
        /// Updates an existing category on database
        /// </summary>
        /// <param name="id">id of the category</param>
        /// <param name="category">category changes</param>
        public async Task<IActionResult> UpdateCategory([FromRoute] Guid id, [FromBody] CategoryModel category)
        {
            var response = await service.UpdateAsync(id, category);

            if (response.HasError)
            {
                return StatusCode(response.Error.Code, new { response.Error.Message });
            }

            return Ok(response.Data);
        }

        [HttpDelete("{id}")]
        /// <summary>
        /// Deletes an existing category on database
        /// </summary>
        /// <param name="id">id of the category</param>
        public async Task<IActionResult> DeleteCategory([FromRoute] Guid id)
        {
            await service.DeleteAsync(id);
            return NoContent();
        }
    }
}
