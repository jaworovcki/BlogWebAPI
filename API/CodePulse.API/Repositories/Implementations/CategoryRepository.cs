using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using CodePulse.API.Repositories.Intrerfaces;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Repositories.Implementations
{
	//Never deal with DTOs in the repository layer
	//DbContext should be injected into the repository
	public class CategoryRepository : ICategoryRepository
	{
		private readonly ApplicationDbContext _dbContext;

		public CategoryRepository(ApplicationDbContext dbContext)
        {
			_dbContext = dbContext;
		}

        public async Task<Category> CreateAsync(Category category)
		{
			_dbContext.Categories.Add(category);
			await _dbContext.SaveChangesAsync();

			return category;
		}

		public async Task<Category> GetByIdAsync(Guid id)
		{
			var category = await _dbContext.Categories.FindAsync(id);

			if (category == null)
			{
				throw new ArgumentNullException("Incorrect id of the category", nameof(id));
			}

			return category;
		}
	}
}
