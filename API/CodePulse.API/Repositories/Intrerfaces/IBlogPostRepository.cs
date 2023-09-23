using CodePulse.API.Models.Domain;

namespace CodePulse.API.Repositories.Intrerfaces
{
	public interface IBlogPostRepository
	{
		Task<IEnumerable<BlogPost>> GetAllAsync();

		Task<BlogPost> GetAsync(Guid id);

		Task<BlogPost> CreateAsync(BlogPost blogPost);

		Task<BlogPost> UpdateAsync(BlogPost blogPost);

		Task DeleteAsync(Guid id);

		Task<BlogPost> GetByUrlHandleAsync(string urlHandle);
	}
}
