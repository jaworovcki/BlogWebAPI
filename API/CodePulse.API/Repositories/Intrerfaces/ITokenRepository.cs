using Microsoft.AspNetCore.Identity;

namespace CodePulse.API.Repositories.Intrerfaces
{
	public interface ITokenRepository
	{
		string GenerateToken(IdentityUser user, List<string> roles);
	}
}
