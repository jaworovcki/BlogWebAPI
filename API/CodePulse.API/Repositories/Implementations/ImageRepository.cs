using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using CodePulse.API.Repositories.Intrerfaces;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Repositories.Implementations
{
	public class ImageRepository : IImageRepository
	{
		private readonly IWebHostEnvironment _environment;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly ApplicationDbContext _dbContext;

		public ImageRepository(IWebHostEnvironment environment,
			IHttpContextAccessor httpContextAccessor,
			ApplicationDbContext dbContext)
        {
			_environment = environment;
			_httpContextAccessor = httpContextAccessor;
			_dbContext = dbContext;
		}

		public async Task<BlogImage> UploadAsync(IFormFile file, BlogImage blogImage)
		{
			var localPath = Path.Combine(_environment.ContentRootPath, "Images", $"{blogImage.FileName}{blogImage.FileExtension}");

			using (var stream = new FileStream(localPath, FileMode.Create))
			{
				await file.CopyToAsync(stream);
			}

			var httpRequest = _httpContextAccessor.HttpContext!.Request;
			var urlPath = $"{httpRequest.Scheme}://{httpRequest.Host}{httpRequest.PathBase}/Images/{blogImage.FileName}{blogImage.FileExtension}";

			blogImage.ImageUrl = urlPath;

			_dbContext.Add(blogImage);
			await _dbContext.SaveChangesAsync();

			return blogImage;
		}

		public async Task<BlogImage> GetAsync(Guid id)
		{
			var blogImage = await _dbContext.BlogImages.FindAsync(id);

			if (blogImage == null)
			{
				throw new ArgumentNullException("Incorrect id of the blog image", nameof(id));
			}

			return blogImage;
		}

		public async Task<IEnumerable<BlogImage>> GetAll() 
			=> await _dbContext.BlogImages
				.AsNoTracking()
				.ToListAsync();
	}
}
