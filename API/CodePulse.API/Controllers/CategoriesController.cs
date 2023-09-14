using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CategoriesController : ControllerBase
	{
		private readonly ApplicationDbContext _context;

		public CategoriesController(ApplicationDbContext context)
        {
			_context = context;
		}

        [HttpPost]
		[ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<CategoryDto>> CreateCategory(CreateCategoryRequestDto categoryDto)
        {
            var newCategory = new Category
            {
				Name = categoryDto.Name,
				UrlHandle = categoryDto.UrlHandle
			};

			_context.Categories.Add(newCategory);
			await _context.SaveChangesAsync();

			var response = new CategoryDto()
			{
				Id = newCategory.Id,
				Name = newCategory.Name,
				UrlHandle = newCategory.UrlHandle
			};

			return CreatedAtAction(nameof(GetCategoryById), new CategoryDto { Id = response.Id }, response);
		}

		[HttpGet("id", Name = nameof(GetCategoryById))]
		public async Task<ActionResult<CategoryDto>> GetCategoryById(Guid id)
		{
			var category = await _context.Categories.FindAsync(id);

			if (category == null)
			{
				return NotFound();
			}

			var categoryDto = new CategoryDto
			{
				Id = category.Id,
				Name = category.Name,
				UrlHandle = category.UrlHandle
			};

			return Ok(categoryDto);
		}
    }
}
