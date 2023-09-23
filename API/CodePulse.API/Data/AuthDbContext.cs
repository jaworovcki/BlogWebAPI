using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace CodePulse.API.Data
{
	public class AuthDbContext : IdentityDbContext
	{
		public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			var roles = new[]
			{
				new IdentityRole
				{
					Id = "fc893f28-d5b1-4407-9831-decbccbd6f0a",
					Name = "Admin",
					NormalizedName = "ADMIN",
				},
				new IdentityRole
				{
					Id = "92e5afc6-2282-4641-80d5-beb410468cc1",
					Name = "User",
					NormalizedName = "USER"
				}
			};

			builder.Entity<IdentityRole>().HasData(roles);

			var admin = new IdentityUser
			{
				Id = "a0f8f6c0-1f1a-4f1e-9f1c-1a2b3c4d5e6f",
				UserName = "admin",
				NormalizedUserName = "ADMIN",
				Email = "andmin@gmail.com",
				NormalizedEmail = "andmin@gmail.com".ToUpper(),
			};

			admin.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(admin, "admin");

			builder.Entity<IdentityUser>().HasData(admin);

			var adminRole = new IdentityUserRole<string>
			{
				RoleId = roles[0].Id,
				UserId = admin.Id,
			};

			builder.Entity<IdentityUserRole<string>>().HasData(adminRole);
		}
	}
}
