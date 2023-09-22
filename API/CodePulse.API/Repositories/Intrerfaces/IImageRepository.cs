using CodePulse.API.Models.Domain;

namespace CodePulse.API.Repositories.Intrerfaces
{
	public interface IImageRepository
	{
		Task<BlogImage> UploadAsync(IFormFile file, BlogImage blogImage);

		Task<BlogImage> GetAsync(Guid id);

		Task<IEnumerable<BlogImage>> GetAll();
	}
}
