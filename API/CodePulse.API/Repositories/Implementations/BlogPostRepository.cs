using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using CodePulse.API.Repositories.Intrerfaces;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Repositories.Implementations
{
	public class BlogPostRepository : IBlogPostRepository
	{
		private readonly ApplicationDbContext _dbContext;

		public BlogPostRepository(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<BlogPost> CreateAsync(BlogPost blogPost)
		{
			_dbContext.BlogPosts.Add(blogPost);
			await _dbContext.SaveChangesAsync();

			return blogPost;
		}

		public async Task DeleteAsync(Guid id)
		{
			var blogPost = await GetAsync(id);

			_dbContext.BlogPosts.Remove(blogPost);
			await _dbContext.SaveChangesAsync();
		}

		public async Task<IEnumerable<BlogPost>> GetAllAsync() => await _dbContext.BlogPosts
				.Include(blogPost => blogPost.Categories)
				.AsNoTracking()
				.ToListAsync();

		public async Task<BlogPost> GetAsync(Guid id)
		{
			var blogPost = await _dbContext.BlogPosts.FindAsync(id);

			if (blogPost == null)
			{
				throw new ArgumentNullException("Incorrect id of the blog post", nameof(id));
			}
			else
			{
				await _dbContext.Entry(blogPost)
					.Collection(blogPost => blogPost.Categories)
					.LoadAsync();
			}

			return blogPost;
		}

		public async Task<BlogPost> GetByUrlHandleAsync(string urlHandle)
		{
			var blogPost = await _dbContext.BlogPosts
				.FirstOrDefaultAsync(blogPost => blogPost.UrlHandle == urlHandle);

			if (blogPost == null)
			{
				throw new ArgumentNullException("Incorrect url handle of the blog post", nameof(urlHandle));
			}
			else
			{
				await _dbContext.Entry(blogPost)
					.Collection(blogPost => blogPost.Categories)
					.LoadAsync();
			}

			return blogPost;
		}

		public async Task<BlogPost> UpdateAsync(BlogPost blogPost)
		{
			var blogPostToUpdate = await GetAsync(blogPost.Id);
			_dbContext.Entry(blogPostToUpdate).CurrentValues.SetValues(blogPost);

			await _dbContext.SaveChangesAsync();
			return blogPostToUpdate;
		}
	}
}
