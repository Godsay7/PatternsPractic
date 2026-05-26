using System;
using BLL.DTO;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly IFinanceService _service;

        public CategoriesController(IFinanceService service)
        {
            _service = service;
        }

        // GET: api/categories
        [HttpGet]
        public IActionResult GetAll()
        {
            var categories = _service.GetAllCategories();
            return Ok(categories);
        }

        // POST: api/categories
        [HttpPost]
        public IActionResult Create([FromBody] CategoryDTO dto)
        {
            try
            {
                _service.CreateCategory(dto);
                return Ok(new { message = "Category created successfully!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] string newName)
        {
            try
            {
                _service.UpdateCategory(id, newName);
                return Ok(new { message = "Category updated successfully!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _service.DeleteCategory(id);
                return Ok(new { message = "Category deleted successfully!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}