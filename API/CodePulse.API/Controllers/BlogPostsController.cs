using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTO;
using CodePulse.API.Repositories.Intrerfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BlogPostsController : ControllerBase
	{
        private readonly IBlogPostRepository _blogPostRepository;
		private readonly ICategoryRepository _categoryRepository;

		public BlogPostsController(IBlogPostRepository blogPostRepository, ICategoryRepository categoryRepository)
		{
			_blogPostRepository = blogPostRepository;
			_categoryRepository = categoryRepository;
		}

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status201Created)]
		public async Task<ActionResult<BlogPostDto>> CreateBlogPost(CreateBlogPostRequestDto requestDto)
		{
			var newBlogPost = new BlogPost
			{
				Author = requestDto.Author,
				Title = requestDto.Title,
				Content = requestDto.Content,
				CreatedDate = requestDto.CreatedDate,
				Description = requestDto.Description,
				FeatureImageURl = requestDto.FeatureImageURl,
				IsVisible = requestDto.IsVisible,
				UrlHandle = requestDto.UrlHandle,
				Categories = new List<Category>(),
			};

			foreach (var categoryId in requestDto.Categories)
			{
				var category = await _categoryRepository.GetByIdAsync(categoryId);
				if (category is not null)
				{
					newBlogPost.Categories.Add(category);
				}
			}

			await _blogPostRepository.CreateAsync(newBlogPost);

			var response = new BlogPostDto()
			{
				Id = newBlogPost.Id,
				Author = newBlogPost.Author,
				Title = newBlogPost.Title,
				Content = newBlogPost.Content,
				CreatedDate = newBlogPost.CreatedDate,
				Description = newBlogPost.Description,
				FeatureImageURl = newBlogPost.FeatureImageURl,
				IsVisible = newBlogPost.IsVisible,
				UrlHandle = newBlogPost.UrlHandle,
				Categories = newBlogPost.Categories.Select(category => new CategoryDto
				{
					Id = category.Id,
					Name = category.Name,
					UrlHandle = category.UrlHandle
				}).ToList()
			};

			return CreatedAtAction(nameof(GetBlogPostById), new BlogPostDto { Id = response.Id }, response);
		}

		[HttpGet("{id}", Name = nameof(GetBlogPostById))]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BlogPostDto))]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<BlogPostDto>> GetBlogPostById([FromRoute] Guid id)
		{
			try
			{
				var blogPost = await _blogPostRepository.GetAsync(id);

				var blogPostDto = new BlogPostDto
				{
					Id = blogPost.Id,
					Author = blogPost.Author,
					Title = blogPost.Title,
					Content = blogPost.Content,
					CreatedDate = blogPost.CreatedDate,
					Description = blogPost.Description,
					FeatureImageURl = blogPost.FeatureImageURl,
					IsVisible = blogPost.IsVisible,
					UrlHandle = blogPost.UrlHandle,
					Categories = blogPost.Categories.Select(category => new CategoryDto
					{
						Id = category.Id,
						Name = category.Name,
						UrlHandle = category.UrlHandle
					}).ToList()
				};

				return Ok(blogPostDto);
			}
			catch (ArgumentNullException)
			{
				return NotFound();
			}
		}

		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<BlogPostDto>))]
		public async Task<ActionResult<IEnumerable<BlogPostDto>>> GetAllBlogPosts()
		{
			var blogPosts = await _blogPostRepository.GetAllAsync();

			var blogPostsDto = blogPosts.Select(blogPost => new BlogPostDto
			{
				Id = blogPost.Id,
				Author = blogPost.Author,
				Title = blogPost.Title,
				Content = blogPost.Content,
				CreatedDate = blogPost.CreatedDate,
				Description = blogPost.Description,
				FeatureImageURl = blogPost.FeatureImageURl,
				IsVisible = blogPost.IsVisible,
				UrlHandle = blogPost.UrlHandle,
				Categories = blogPost.Categories.Select(category => new CategoryDto
				{
					Id = category.Id,
					Name = category.Name,
					UrlHandle = category.UrlHandle
				}).ToList(),
			});

			return Ok(blogPostsDto);
		}

		[HttpDelete("{id}")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult> DeleteBlogPost([FromRoute] Guid id)
		{
			try
			{
				await _blogPostRepository.DeleteAsync(id);
				return NoContent();
			}
			catch (ArgumentNullException)
			{
				return NotFound();
			}
		}

		[HttpPut("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<BlogPostDto>> UpdateBlogPost([FromRoute] Guid id, [FromBody] UpdateBlogPostRequestDto requestDto)
		{
			try
			{
				var blogPost = new BlogPost()
				{
					Id = id,
					Author = requestDto.Author,
					Title = requestDto.Title,
					Content = requestDto.Content,
					CreatedDate = requestDto.CreatedDate,
					Description = requestDto.Description,
					FeatureImageURl = requestDto.FeatureImageURl,
					IsVisible = requestDto.IsVisible,
					UrlHandle = requestDto.UrlHandle
				};

				await _blogPostRepository.UpdateAsync(blogPost);

				var response = new BlogPostDto()
				{
					Id = blogPost.Id,
					Author = blogPost.Author,
					Title = blogPost.Title,
					Content = blogPost.Content,
					CreatedDate = blogPost.CreatedDate,
					Description = blogPost.Description,
					FeatureImageURl = blogPost.FeatureImageURl,
					IsVisible = blogPost.IsVisible,
					UrlHandle = blogPost.UrlHandle
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
