using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api.DAL;

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
        [HttpGet]

        public async Task<IActionResult> GetCategories()
        {
            return Ok(await _db.Categories.ToListAsync());
        }
    }
}