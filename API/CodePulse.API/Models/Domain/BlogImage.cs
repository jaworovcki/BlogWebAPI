namespace CodePulse.API.Models.Domain
{
	public class BlogImage
	{
		public Guid Id { get; set; }

		public string FileName { get; set; } = string.Empty;

		public string FileExtension { get; set; } = string.Empty;

		public string ImageUrl { get; set; } = string.Empty;

		public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    }
}
