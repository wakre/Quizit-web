using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api.DAL;
using api.DTOs;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly AppDbContext _db;

        public CategoryController(AppDbContext db)
        {
            _db = db;
        }

        // Get all categories (manual mapping to DTO)
        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _db.Categories.AsNoTracking().ToListAsync();

            if (categories == null)
                return StatusCode(500, "Error retrieving categories.");

            // MANUAL MAPPING: Entity -> DTO
            var categoryDtos = categories.Select(c => new CategoryDto
            {
                CategoryId = c.CategoryId,
                Name = c.Name
            }).ToList();

            return Ok(categoryDtos);
        }
    }
}
