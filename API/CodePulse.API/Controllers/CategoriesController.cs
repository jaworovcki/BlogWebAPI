using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTO;
using CodePulse.API.Repositories.Intrerfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CategoriesController : ControllerBase
	{
		private readonly ICategoryRepository _categoryRepository;

		public CategoriesController(ICategoryRepository categoryRepository)
		{
			_categoryRepository = categoryRepository;
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

			await _categoryRepository.CreateAsync(newCategory);

			var response = new CategoryDto()
			{
				Id = newCategory.Id,
				Name = newCategory.Name,
				UrlHandle = newCategory.UrlHandle
			};

			return CreatedAtAction(nameof(GetCategoryById), new CategoryDto { Id = response.Id }, response);
		}

		[HttpGet("id", Name = nameof(GetCategoryById))]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CategoryDto))]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<CategoryDto>> GetCategoryById(Guid id)
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
	}
}
