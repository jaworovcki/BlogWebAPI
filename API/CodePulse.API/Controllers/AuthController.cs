using CodePulse.API.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly UserManager<IdentityUser> _userManager;

		public AuthController(UserManager<IdentityUser> userManager)
		{
			_userManager = userManager;
		}

		[HttpPost("register")]
		public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
		{
			var user = new IdentityUser()
			{
				UserName = request.Email,
				Email = request.Email,
			};

			var identityResult = await _userManager.CreateAsync(user, request.Password);

			if (identityResult.Succeeded)
			{
				var roleResult = await _userManager.AddToRoleAsync(user, "User");

				if (roleResult.Succeeded)
				{
					return Ok();
				}
				else
				{
					foreach (var error in roleResult.Errors)
					{
						ModelState.AddModelError(error.Code, error.Description);
					}
				}
			}
			else 
			{
				foreach (var error in identityResult.Errors)
				{
					ModelState.AddModelError(error.Code, error.Description);
				}
			}

			return ValidationProblem(ModelState);

		}
	}
}
