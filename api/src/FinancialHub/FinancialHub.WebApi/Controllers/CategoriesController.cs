﻿using FinancialHub.Domain.Interfaces.Services;
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
            try
            {
                var response = await service.GetAllByUserAsync("mock");
                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(ICollection<CategoryModel>), 200)]
        /// <summary>
        /// Creates an category on database (will be changed to only one user)
        /// </summary>
        /// <param name="category">Account to be created</param>
        public async Task<IActionResult> CreateCategory(CategoryModel category)
        {
            try
            {
                var response = await service.CreateAsync(category);
                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ICollection<CategoryModel>), 200)]
        /// <summary>
        /// Updates an existing category on database
        /// </summary>
        /// <param name="id">id of the category</param>
        /// <param name="category">category changes</param>
        public async Task<IActionResult> UpdateCategory(string id, CategoryModel category)
        {
            try
            {
                var response = await service.UpdateAsync(id, category);
                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpDelete("{id}")]
        /// <summary>
        /// Deletes an existing category on database
        /// </summary>
        /// <param name="id">id of the category</param>
        public async Task<IActionResult> DeleteCategory(string id)
        {
            try
            {
                await service.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}