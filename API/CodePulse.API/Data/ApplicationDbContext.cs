using CodePulse.API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Data
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{
		}

		public DbSet<BlogPost> BlogPosts { get; set; } = null!;

		public DbSet<Category> Categories { get; set; } = null!;

		public DbSet<BlogImage> BlogImages { get; set; } = null!;
	}
}
