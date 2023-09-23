using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTO;
using CodePulse.API.Repositories.Intrerfaces;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ImagesController : ControllerBase
	{
		private readonly IImageRepository _imageRepository;

		public ImagesController(IImageRepository imageRepository)
		{
			_imageRepository = imageRepository;
		}

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status201Created, Type = typeof(BlogImageDto))]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<BlogImageDto>> UploadImage([FromForm] IFormFile file,
			[FromForm] string fileName)
		{
			ValidateFile(file);

			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var image = new BlogImage
			{
				FileName = fileName,
				FileExtension = Path.GetExtension(file.FileName).ToLower(),
			};

			var blogImage = await _imageRepository.UploadAsync(file, image);

			var response = new BlogImageDto()
			{
				Id = blogImage.Id,
				FileName = blogImage.FileName,
				FileExtension = blogImage.FileExtension,
				ImageUrl = blogImage.ImageUrl,
			};

			return CreatedAtAction(nameof(GetImage), new { Id = response.Id}, response);
		}

		[HttpGet("{id}", Name = nameof(GetImage))]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BlogImageDto))]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<BlogImageDto>> GetImage(Guid id)
		{
			try
			{
				var image = await _imageRepository.GetAsync(id);

				var response = new BlogImageDto()
				{
					Id = image.Id,
					FileName = image.FileName,
					FileExtension = image.FileExtension,
					ImageUrl = image.ImageUrl,
				};

				return Ok(response);
			}
			catch(ArgumentNullException)
			{
				return NotFound();
			}
		}

		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<BlogImageDto>))]
		public async Task<ActionResult<IEnumerable<BlogImageDto>>> GetAllImages()
		{
			var images = await _imageRepository.GetAll();

			var response = images.Select(image => new BlogImageDto()
			{
				Id = image.Id,
				FileName = image.FileName,
				FileExtension = image.FileExtension,
				ImageUrl = image.ImageUrl,
			});

			return Ok(response);
		}

		private void ValidateFileLength(IFormFile file)
		{
			if (file == null || file.Length == 0)
			{
				ModelState.AddModelError("File", "File cannot be empty");
			}
			else if (file.Length > 10 * 1024 * 1024)
			{
				ModelState.AddModelError("FileSize", "File cannot be larger than 10MB");
			}
		}

		private void ValidateFileExtension(IFormFile file)
		{
			var extension = Path.GetExtension(file.FileName).ToLower();

			var allowedExtensions = new[] { ".jpg", ".png", ".gif" };

			if (!allowedExtensions.Contains(extension))
			{
				ModelState.AddModelError("FileExtension", "File extension is not allowed");
			}
		}

		private void ValidateFile(IFormFile file)
		{
			ValidateFileLength(file);
			ValidateFileExtension(file);
		}
	}
}
