using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTO;
using CodePulse.API.Repositories.Intrerfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class CategoriesController : ControllerBase
	{
		private readonly ICategoryRepository _categoryRepository;

		public CategoriesController(ICategoryRepository categoryRepository)
		{
			_categoryRepository = categoryRepository;
		}

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[Authorize(Roles = "Admin")]
		public async Task<ActionResult<CategoryDto>> CreateCategory(CreateCategoryRequestDto categoryDto)
		{
			var newCategory = new Category
			{
				Name = categoryDto.Name,
				UrlHandle = categoryDto.UrlHandle
			};

			await _categoryRepository.CreateAsync(newCategory);

			var response = new CategoryDto()
			{
				Id = newCategory.Id,
				Name = newCategory.Name,
				UrlHandle = newCategory.UrlHandle
			};

			return CreatedAtAction(nameof(GetCategoryById), new CategoryDto { Id = response.Id }, response);
		}

		[HttpGet("{id}", Name = nameof(GetCategoryById))]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CategoryDto))]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<CategoryDto>> GetCategoryById([FromRoute] Guid id)
		{
			try
			{
				var category = await _categoryRepository.GetByIdAsync(id);

				var categoryDto = new CategoryDto
				{
					Id = category.Id,
					Name = category.Name,
					UrlHandle = category.UrlHandle
				};

				return Ok(categoryDto);
			}
			catch (ArgumentNullException)
			{
				return NotFound();
			}
		}

		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CategoryDto>))]
		public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAllCategories()
		{
			var categories = await _categoryRepository.GetAllAsync();

			var categoriesDto = categories.Select(category => new CategoryDto
			{
				Id = category.Id,
				Name = category.Name,
				UrlHandle = category.UrlHandle
			});

			return Ok(categoriesDto);
		}

		[HttpDelete("{id}")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> DeleteCategoryById([FromRoute] Guid id)
		{
			try
			{
				await _categoryRepository.DeleteByIdAsync(id);

				return NoContent();
			}
			catch (ArgumentNullException)
			{
				return NotFound();
			}
		}

		[HttpPut("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CategoryDto))]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[Authorize(Roles = "Admin")]
		public async Task<ActionResult<CategoryDto>> UpdateCategory([FromRoute] Guid id, [FromBody] UpdateCategoryRequestDto request)
		{
			try
			{
				var category = new Category
				{
					Id = id,
					Name = request.Name,
					UrlHandle = request.UrlHandle
				};

				var updatedCategory = await _categoryRepository.UpdateAsync(category);

				var response = new CategoryDto
				{
					Id = updatedCategory.Id,
					Name = updatedCategory.Name,
					UrlHandle = updatedCategory.UrlHandle
				};

				return Ok(response);
			}
			catch (ArgumentNullException)
			{
				return NotFound();
			}
		}
	}
}
